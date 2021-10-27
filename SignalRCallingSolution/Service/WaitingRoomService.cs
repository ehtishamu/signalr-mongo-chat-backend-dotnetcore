using SignalRCallingSolution.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SignalRCallingSolution.Service
{
    public class WaitingRoomService:IWaitingRoomService
    {

        #region Encryption Decryption
        private string DecryptString(string base64StringToDecrypt)
        {
            string passphrase = "2595874569321569";
            var len = base64StringToDecrypt.Length;
            base64StringToDecrypt = base64StringToDecrypt.Replace('!', '+');
            base64StringToDecrypt = base64StringToDecrypt.Replace(' ', '+');
            len = base64StringToDecrypt.Length;
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(passphrase)))
            {
                byte[] RawBytes = Convert.FromBase64String(base64StringToDecrypt);
                ICryptoTransform ictD = acsp.CreateDecryptor();
                MemoryStream msD = new MemoryStream(RawBytes, 0, RawBytes.Length);
                CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read);
                var a = (new StreamReader(csD)).ReadToEnd();
                return a;
            }
        }
        private static AesCryptoServiceProvider GetProvider(byte[] key)
        {
            AesCryptoServiceProvider result = new AesCryptoServiceProvider();
            result.BlockSize = 128;
            result.KeySize = 128;
            result.Mode = CipherMode.CBC;
            result.Padding = PaddingMode.PKCS7;

            result.GenerateIV();
            result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            byte[] RealKey = GetKey(key, result);
            result.Key = RealKey;
            // result.IV = RealKey;
            return result;
        }
        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            byte[] kRaw = suggestedKey;
            List<byte> kList = new List<byte>();

            for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                kList.Add(kRaw[(i / 8) % kRaw.Length]);
            }
            byte[] k = kList.ToArray();
            return k;
        }
        #endregion

        public WaitingRoomCallDetail urlDecrypt(string url)
        {
            WaitingRoomCallDetail callDetail = new WaitingRoomCallDetail();

            string realdata = DecryptString(url);
            string[] waitingRoomData = realdata.Split('#');
            callDetail.patientAccount = waitingRoomData[0].ToString();
            callDetail.patientName = ToTitleCase(waitingRoomData[1].ToString()) + " " + ToTitleCase(waitingRoomData[2].ToString());
            string providerName = ToTitleCase(waitingRoomData[3].ToString()) + " " + ToTitleCase(waitingRoomData[4].ToString());
            string providerTitle = waitingRoomData[5].ToString();
            if (providerTitle != "EMPTY")
            {
                if (providerTitle == "Dr")
                {
                    providerName = providerTitle + " " + providerName;
                }
                else
                {
                    providerName = providerName + " " + providerTitle;
                }
            }
            callDetail.providerName = providerName;
            callDetail.providerTitle = providerTitle;

            callDetail.appointmentID = waitingRoomData[6].ToString();
            callDetail.appointmentDateTime = waitingRoomData[7].ToString();
            callDetail.practiceCode = waitingRoomData[8].ToString();
            callDetail.providerCode = waitingRoomData[9].ToString();
            callDetail.perspective = waitingRoomData[10].ToString();
            callDetail.participantName = waitingRoomData[11].ToString();
            if (waitingRoomData.Length>12)
            {
                if (waitingRoomData[12] != null)
                callDetail.isCareClinic = System.Convert.ToBoolean(waitingRoomData[12].ToString());
            }

            return callDetail;

        }
        private static string ToTitleCase(string str)
        {
            string formattedText = null;

            if (str != null)
            {
                formattedText = new System.Globalization.CultureInfo("en").TextInfo.ToTitleCase(str.ToLower());
            }
            return formattedText;

        }
      
    }
}
