using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialSquirrel.Models
{
    public class Post
    {
        public string Message { get; set; }

        public string UserName { get; set; }

        public DateTime CreationDate { get; set; }

        public string FormattedCreationDate
        {
            get
            {
                if (CreationDate == null) { return string.Empty; }

                TimeSpan differenceSpan = DateTime.Now.Subtract(CreationDate);
                string denominator =
                  differenceSpan.Days > 0 ? "day" :
                  differenceSpan.Hours > 0 ? "hour" :
                  differenceSpan.Minutes > 0 ? "minute" : "second";

                string formattedCreationDate = String.Empty;

                int numeral;

                if (denominator == "day")
                {
                    formattedCreationDate = String.Format("{0:MMM dd} at {0:HH}:{0:mm}", CreationDate);
                }
                else
                {
                    switch (denominator)
                    {
                        case "second":
                            numeral = differenceSpan.Seconds;
                            break;
                        case "minute":
                            numeral = differenceSpan.Minutes;
                            break;
                        default:
                            numeral = differenceSpan.Hours;
                            break;
                    }

                    denominator += (numeral > 1) ? "s" : String.Empty;
                    formattedCreationDate = String.Format("{0} {1} ago", numeral, denominator);
                }
                return formattedCreationDate;

                //REFERENCE : http://stackoverflow.com/questions/2139022/format-datetime-like-stackoverflow viewed on 01/07/2014
            }
        }

        public static Post CreateNew(string message, string userName)
        {
            return new Post()
            {
                Message = message,
                UserName = userName,
                CreationDate = DateTime.Now                
            };
        }
    }
}
