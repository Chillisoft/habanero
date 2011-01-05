using System;

using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Habanero.Base.Util
{
    public class RSAExtenderCF
    {
        RSA _rsa;

        public RSAExtenderCF(RSA rsa)
        {
            _rsa = rsa;
        }

        public void FromXmlString(string xmlString)
        {
            RSAParameters p;
            if (xmlString == null) throw new
            ArgumentNullException("xmlString");

            p = new RSAParameters();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            XmlNode modulus = doc.SelectSingleNode("//Modulus");
            if (modulus == null) throw new CryptographicException();
            p.Modulus = Convert.FromBase64String(modulus.InnerText);

            XmlNode exponent = doc.SelectSingleNode("//Exponent");
            if (modulus == null) throw new CryptographicException();
            p.Exponent = Convert.FromBase64String(exponent.InnerText);

            XmlNode pNode = doc.SelectSingleNode("//P");
            if (pNode != null)
                p.P = Convert.FromBase64String(pNode.InnerText);

            XmlNode qNode = doc.SelectSingleNode("//Q");
            if (qNode != null)
                p.Q = Convert.FromBase64String(qNode.InnerText);

            XmlNode dpNode = doc.SelectSingleNode("//DP");
            if (dpNode != null)
                p.DP = Convert.FromBase64String(dpNode.InnerText);

            XmlNode dqNode = doc.SelectSingleNode("//DQ");
            if (dqNode != null)
                p.DQ = Convert.FromBase64String(dqNode.InnerText);

            XmlNode inverseQNode = doc.SelectSingleNode("//InverseQ");
            if (inverseQNode != null)
                p.InverseQ = Convert.FromBase64String(inverseQNode.InnerText);

            XmlNode dNode = doc.SelectSingleNode("//D");
            if (dNode != null)
                p.D = Convert.FromBase64String(dNode.InnerText);

            _rsa.ImportParameters(p);
        }

        public string ToXmlString(bool includePrivateParameters)
        {
            RSAParameters p = _rsa.ExportParameters(includePrivateParameters);
            StringBuilder sb = new StringBuilder();

            sb.Append("<RSAKeyValue>");
            sb.Append("<Modulus>" + Convert.ToBase64String(p.Modulus) +
            "</Modulus>");
            sb.Append("<Exponent>" + Convert.ToBase64String(p.Exponent) +
            "</Exponent>");

            if (includePrivateParameters)
            {
                sb.Append("<P>" + Convert.ToBase64String(p.P) + "</P>");
                sb.Append("<Q>" + Convert.ToBase64String(p.Q) + "</Q>");
                sb.Append("<DP>" + Convert.ToBase64String(p.DP) + "</DP>");
                sb.Append("<DQ>" + Convert.ToBase64String(p.DQ) + "</DQ>");
                sb.Append("<InverseQ>" + Convert.ToBase64String(p.InverseQ)
                + "</InverseQ>");
                sb.Append("<D>" + Convert.ToBase64String(p.D) + "</D>");
            }

            sb.Append("</RSAKeyValue>");
            return sb.ToString();
        }


    }

}
