using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Integrator.Models
{
    [Serializable]
    public class CardInformation
    {
        string _cardNumber = "";
        public string CardNumber {
            get {
                return _cardNumber;
            }
            set {
                _cardNumber = Regex.Replace(value, @"\s", "" );
            }
        }

        string _cvv = "";
        public string Cvv {
            get {
                return _cvv;
            }
            set {
                _cvv = Regex.Replace(value, @"\s", "");
            }
        }

        string _holderName = "";
        public string HolderName {
            get {
                return _holderName;
            }
            set {
                _holderName = value.Trim();
            }
        }

        public string _expiryDate = "";
        public string ExpiryDate {
            get {
                return _expiryDate;
            }
            set {
                if (value.IndexOf('/') > -1) {
                    value = value.Replace('/', '.');                 
                }

                _expiryDate = Regex.Replace(value, @"\s", "");
            }
        }
    }
}
