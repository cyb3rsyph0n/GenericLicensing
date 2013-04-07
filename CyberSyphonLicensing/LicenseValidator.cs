using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CyberSyphonLicensing
{
    public static class LicenseValidator
    {
        /// <summary>
        /// VALIDATES A KEY THAT HAS BEEN GENERATED FOR THE GIVEN APPLICATION ID
        /// </summary>
        /// <param name="AppID">APPLICATION THE KEY SHOULD BE VALIDATED AGAINST</param>
        /// <param name="Key">UNIQUE KEY TO COMPARE AGAINST</param>
        /// <returns>TRUE OR FALSE REPRESENTING THE VALIDATION OF THE GIVEN KEY AND APPLICATION ID</returns>
        public static bool ValidateKey(string Key)
        {
            try
            {
                int curSegment = 1;
                string[] segments = Key.ToUpper().Split(new char[] { '-' });
                string AppID = ((GuidAttribute)Assembly.GetCallingAssembly().GetCustomAttributes(typeof(GuidAttribute), false)[0]).Value;

                foreach (string segment in segments)
                {
                    //SPLIT THE SEGMENT INTO HASH VALUE AND KEY VALUE
                    string hashedValue = segment.Substring(0, 2);
                    string keyValue = segment.Substring(2, 2);

                    //COMPUTE THE HASH USING THE REVERSE ALGORITHM FROM CREATING A HASH
                    string hash = BitConverter.ToString(SHA256.Create().ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(keyValue + AppID)).Skip((keyValue.ToCharArray()[0] + curSegment++) % 32).Take(1).ToArray());

                    //IF THE HASH DOES NOT MATCH WHAT IS BELIEVED SHOULD BE A MATCH THEN DROP OUT BECAUSE THE KEY IS INVALID
                    if (hash != hashedValue)
                        return false;
                }

                //IF WE MADE IT THIS FAR THEN THE KEY IS VALID
                return true;
            }
            catch
            {
                //IF THERE WAS AN ERROR WHILE ATTEMPTING TO VALIDATE THEN THE KEY MUST BE INVALID
                return false;
            }
        }
    }
}
