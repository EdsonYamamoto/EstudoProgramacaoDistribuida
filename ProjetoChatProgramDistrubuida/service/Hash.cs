using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace ProjetoChatProgramDistrubuida.service
{
    class Hash
    {

        public static void Teste()
        {
            /*
            string version = "02000000";
            string previousBlock = "975b9717f7d18ec1f2ad55e2559b5997b8da0e3317c803780000000100000000";
            string merkleRoot = "0000000000000000e067a478024addfecdc93628978aa52d91fabd4292982a501234569876 ";

            string timestamp = "9876";
            string bits = "535f0119";
            string nonce = "123456";
            string transactionCounta = "63";
            */

            string resposta = "68763dc34ed271a0d738c22592a2d4e8f38f9d477aa4d7951c7cf933835e190e";
            string job_id = "58af8d8c";
            string prevhash = "975b9717f7d18ec1f2ad55e2559b5997b8da0e3317c803780000000100000000";
            string coinb1 = "01000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4803636004062f503253482f04428b055308";
            string coinb2 = "2e522cfabe6d6da0bd01f57abe963d25879583eea5ea6f08f83e3327eba9806b14119718cbb1cf04000000000000000000000001fb673495000000001976a91480ad90d403581fa3bf46086a91b2d9d4125db6c188ac00000000";
            string merkle_branch = "['ea9da84d55ebf07f47def6b9b35ab30fc18b6e980fc618f262724388f2e9c591', ...]";
            string version = "00000002";
            string nbits = "19015f53";
            string ntime = "53058b41";
            bool clean_jobs = false;
            /*
            while (nonce < 0x100000000)
            {
                header = ( struct.pack("<L", ver) + prev_block.decode('hex')[::-1] + mrkl_root.decode('hex')[::-1] + struct.pack("<LLL", time_, bits, nonce))
                hash = hashlib.sha256(hashlib.sha256(header).digest()).digest()
                print nonce, hash[::- 1].encode('hex')

                if hash[::- 1] < target_str:
                    print 'success'
                    break
                nonce += 1
            }
            */

            /*
                # https://en.bitcoin.it/wiki/Difficulty
                exp = bits >> 24
                mant = bits & 0xffffff
                target_hexstr = '%064x' % (mant * (1<<(8*(exp - 3))))
                target_str = target_hexstr.decode('hex')

                nonce = 0
                while nonce < 0x100000000:
                header = ( struct.pack("<L", ver) + prev_block.decode('hex')[::-1] +
                  mrkl_root.decode('hex')[::-1] + struct.pack("<LLL", time_, bits, nonce))
                hash = hashlib.sha256(hashlib.sha256(header).digest()).digest()
                print nonce, hash[::-1].encode('hex')
                if hash[::-1] < target_str:
                print 'success'
                break
                nonce += 1

             */

            Console.WriteLine (GetHashString("ola mundo")  );

            Console.ReadLine();
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
