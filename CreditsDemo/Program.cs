using Chaos.NaCl;
using NodeApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Transport;

namespace CreditsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var ipAddress = System.Configuration.ConfigurationManager.AppSettings["IpAddress"];

            var publicKey = "[Source PubKey]";
            var privateKey = System.Configuration.ConfigurationManager.AppSettings["PrivateKey"];

            var publicKeyBytes = SimpleBase.Base58.Bitcoin.Decode(publicKey).ToArray();
            var privateKeyBytes = SimpleBase.Base58.Bitcoin.Decode(privateKey).ToArray();

            var targetPublicKey = "[Target PubKey]";
            var targetPublicKeyBytes = SimpleBase.Base58.Bitcoin.Decode(targetPublicKey).ToArray();

            Task.Run(async () =>
            {
                try
                {
                    using (var transport = new TSocket(ipAddress, 9090))
                    {
                        using (var protocol = new TBinaryProtocol(transport))
                        {
                            using (var client = new API.Client(protocol))
                            {
                                transport.Open();

                                #region GetBalance
                                //Get the balance of our wallet.
                                var balance = client.BalanceGet(publicKeyBytes, 0);
                                Console.WriteLine($"[{publicKey}] Balance: {ConvUtils.FormatAmount(balance.Amount)} CS");
                                #endregion
                                Debugger.Break();

                                #region GetTransactions
                                //Get transactions

                                var transactionGetResult = client.TransactionsGet(publicKeyBytes, 0, 5);
                                #endregion
                                Debugger.Break();

                                #region CreateTransaction
                                //Create a transaction
                                var transaction = new Transaction();
                                transaction.Id = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                                transaction.Source = publicKeyBytes;
                                transaction.Target = targetPublicKeyBytes;
                                transaction.Amount = new Amount(5, 0);
                                transaction.Balance = balance.Amount;
                                transaction.Fee = new Amount(0, 0);
                                transaction.Currency = 1;

                                //Create the signature of the transaction by writing the values into a memory stream and then writes the contents out into a bytearray.
                                byte[] bytes;
                                using (var memoryStream = new MemoryStream())
                                {
                                    using (BinaryWriter writer = new BinaryWriter(memoryStream))
                                    {
                                        writer.Write(transaction.Id);
                                        writer.Write(transaction.Source);
                                        writer.Write(transaction.Target);
                                        writer.Write(transaction.Amount.Integral);
                                        writer.Write(transaction.Amount.Fraction);
                                        writer.Write(transaction.Fee.Integral);
                                        writer.Write(transaction.Fee.Fraction);
                                        writer.Write(transaction.Currency);
                                        writer.Write(0);
                                    }
                                    bytes = memoryStream.ToArray();
                                }

                                var lastHash = SimpleBase.Base58.Bitcoin.Encode(client.GetLastHash());

                                //Sign the bytearray with the privateKey
                                var signature = Ed25519.Sign(bytes, privateKeyBytes);

                                //Verify the signature is correct.
                                var verifyResult = Ed25519.Verify(signature, bytes, SimpleBase.Base58.Bitcoin.Decode(publicKey).ToArray());
                                if (!verifyResult) throw new Exception("Signature could not be verified");

                                transaction.Signature = signature;
                                var transactionResult = client.TransactionFlow(transaction);
                                #endregion
                                Debugger.Break();

                                await Task.Delay(10);
                            }
                        }
                    }
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            });
            Console.ReadKey();
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}



















//var poolHash = ByteArrayToString(lastTransaction.Id.PoolHash);


//bool foundTransaction = false;
//while (!foundTransaction)
//{
//    var transactionGetResult = client.TransactionsGet(publicKeyBytes, 0, 10);
//    var lastTransaction = transactionGetResult.Transactions.FirstOrDefault();
//    var poolHash = ByteArrayToString(lastTransaction.Id.PoolHash);
//    if (transaction.Id == lastTransaction.Trxn.Id)
//    {
//        Console.WriteLine($"Transaction found on chain: {poolHash}.{lastTransaction.Id.Index + 1}");
//        foundTransaction = true;
//    }
//    await Task.Delay(10);
//}