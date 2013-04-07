using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace CyberSyphonKeyGenerator
{
    public partial class frmMain : Form
    {
        /// <summary>
        /// GENERATE A UNIQUE KEY THAT CAN BE REVERESED TO VALIDATE WITHOUT INTERNET CONNECTION
        /// </summary>
        /// <param name="AppID">APPLIATION THE KEY SHOULD BE GERNEATED FOR</param>
        /// <returns>STRING VALUE REPRESENTING A VALID KEY</returns>
        static string GenerateKey(string AppID)
        {
            int curSegment = 1;
            string newLicense = "";
            string License = new string(Guid.NewGuid().ToString().Replace("-", "").ToUpper().ToCharArray().Take(12).ToArray());

            //GENERATE 4 BYTE SEGMENTS WHICH ARE [HASH,HASH][KEY,KEY]
            //VALUES ARE TAKEN FROM THE HASH AT A MATHIMATICALLY COMPUTED LOCATION [(FIRST CHAR ASCII VALUE + SEGMENT) % 32] TO MAKE IT DEPENDENT ON SEGMENT LOCATION AND VALUE
            //THIS SHOULD MAKE IT HARDER TO "GUESS" A KEY OR BRUTE FORCE ONE OUT OF THE SYSTEM
            foreach (Match tmpMatch in Regex.Matches(License, "[a-zA-Z0-9]{2}"))
                newLicense += (newLicense != "" ? "-" : "") + BitConverter.ToString(SHA256.Create().ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(tmpMatch.Value + AppID)).Skip((tmpMatch.Value.ToCharArray()[0] + curSegment++) % 32).Take(1).ToArray()).Replace("-", "") + tmpMatch.Value;

            //RETURN THE NEW LICENSE VALUE TO THE CALLING FUNCTION
            return newLicense;
        }

        public frmMain()
        {
            InitializeComponent();
        }
        
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            txtKey.Text = GenerateKey(txtAppGuid.Text);
        }
    }
}
