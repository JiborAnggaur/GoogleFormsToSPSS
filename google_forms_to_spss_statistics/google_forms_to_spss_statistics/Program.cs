using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace google_forms_to_spss_statistics
{
    class utils
    {
        public static int numOfSubstringInString(string strSource, string str)
        {
            int num = strSource.Length;
            strSource = strSource.Replace(str, "");
            num = num - strSource.Length;
            return num;
        }
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                if (End > Start)
                    return strSource.Substring(Start, End - Start);
            }

            return "";
        }
        public static string getBetween(string strSource, string strEnd)
        {
            if (strSource.Contains(strEnd))
            {
                int Start, End;
                Start = 0;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }
        public static string cutBegin(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource = strSource.Substring(End + strEnd.Length, strSource.Length - End - strEnd.Length);
            }

            return "";
        }
        public static string cutBegin(string strSource, string strEnd)
        {
            if (strSource.Contains(strEnd))
            {
                int End;
                End = strSource.IndexOf(strEnd, 0);
                return strSource = strSource.Substring(End + strEnd.Length, strSource.Length - End - strEnd.Length);
            }

            return "";
        }
        public static string getURI(string uri)
        {
            string result = "";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = client.GetAsync(uri).Result)
                    {
                        using (HttpContent content = response.Content)
                        {
                            result = content.ReadAsStringAsync().Result;
                        }
                    }
                }
            }
            catch { }
            return result;
        }
        public static string replaceUnicodeInString(string Source)
        {
            int position, End;
            int skip = 0;
            while (Source.Contains("\\u"))
            {
                
                    int lenBefore = Source.Length;
                    position = Source.IndexOf("\\u", 0) + 2;
                    for (int j = 0; j < skip; j++)
                    {
                        End = Source.IndexOf("\\u", position) + 2;
                        position = Source.IndexOf("\\u", End) + 2;
                    }
                if (position != -1)
                {
                    try
                    {
                        string replacedString = Source.Substring(position, 4);
                        string s = replacedString.ToUpper();
                        int i = Convert.ToInt32(s, 16);
                        string replaceToChar = Convert.ToString(Convert.ToChar(i));
                        replacedString = "\\u" + replacedString;
                        Source = Source.Replace(replacedString, replaceToChar);
                        int lenAfter = Source.Length;
                        if (lenBefore == lenAfter)
                            skip += 1;
                    }
                    catch
                    {
                        skip += 1;
                        continue;
                    }

                }
                else
                {
                    return Source;
                }
                
            }
            return Source;
        }
        public static string GetFirstBytes(string str, int len)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            while (buffer.Length > len)
            {
                str = str.Substring(0, str.Length - 1);
                buffer = System.Text.Encoding.UTF8.GetBytes(str);
            }
            return str;
        }

        public static List<byte> addToByte(List<byte> bytes, int len, byte[] buffer)
        {
            for (int i = 0; i < len; i++)
            {
                if (i < buffer.Length)
                {
                    bytes.Add(buffer[i]);
                }
                else
                {
                    bytes.Add(0x20);
                }
            }
            return bytes;
        }
        public static List<byte> addToByteNoCut(List<byte> bytes, int len, byte[] buffer)
        {
            if (len >= buffer.Length)
            {
                bytes.AddRange(buffer);
                for (int i = 0; i < len - buffer.Length; i++)
                {
                    bytes.Add(0x20);
                }
            }
            else
            {
                throw new InvalidOperationException("Length of array greater than added length");
            }
            return bytes;
        }
        public static List<byte> addGap(List<byte> bytes)
        {
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            return bytes;
        }
        public static List<byte> addPrefixQuestion(List<byte> bytes)
        {
            bytes.Add(0x02);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x00); //?lenght?
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x01);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x00);
            bytes.Add(0x08); //lenght?
            bytes.Add(0x05); //?type?
            bytes.Add(0x00);

            bytes.Add(0x00);
            bytes.Add(0x08); //lenght?
            bytes.Add(0x05); //?type?
            bytes.Add(0x00);
            return bytes;
        }
        public static List<byte> addPrefixQuestion(List<byte> bytes, bool isString)
        {
            bytes.Add(0x02);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            if (isString)
            {
                bytes.Add(0xFF); //?lenght?
            }
            else
            {
                bytes.Add(0x00); //?lenght?
            }
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x01);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x00);
            if (isString)
            {
                bytes.Add(0x08); //lenght?
                bytes.Add(0x01); //?type?
            }
            else
            {
                bytes.Add(0xFF); //lenght?
                bytes.Add(0x05); //?type?
            }
            bytes.Add(0x00);

            bytes.Add(0x00);
            bytes.Add(0x08); //lenght?
            if (isString)
            {
                bytes.Add(0xFF); //lenght?
                bytes.Add(0x01); //?type?
            }
            else
            {
                bytes.Add(0x08); //lenght?
                bytes.Add(0x05); //?type?
            }
            bytes.Add(0x00);
            return bytes;
        }
        public static List<byte> addPrefixOther(List<byte> bytes)
        {
            bytes.Add(0x02);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            //bytes.Add(0x08); 
            bytes.Add(0xFF); //?lenght?
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x01);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x00);
            //bytes.Add(0x08); 
            bytes.Add(0xFF); //?lenght?
            bytes.Add(0x01); //?type?
            bytes.Add(0x00);

            bytes.Add(0x00);
            //bytes.Add(0x08); 
            bytes.Add(0xFF); //?lenght?
            bytes.Add(0x01); //?type??
            bytes.Add(0x00);
            return bytes;
        }
        public static List<byte> addPostfixOther(List<byte> bytes)
        {
            for (int i = 0; i < 31; i++)
            {
                bytes.Add(0x02);
                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0xFF);
                bytes.Add(0xFF);
                bytes.Add(0xFF);
                bytes.Add(0xFF);
                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0x01);
                bytes.Add(0x1D);
                bytes.Add(0x01);
                bytes.Add(0x00);
                bytes.Add(0x01);
                bytes.Add(0x1D);
                bytes.Add(0x01);
                bytes.Add(0x00);
                bytes.Add(0x20);
                bytes.Add(0x20);
                bytes.Add(0x20);
                bytes.Add(0x20);
                bytes.Add(0x20);
                bytes.Add(0x20);
                bytes.Add(0x20);
                bytes.Add(0x20);
            }

            return bytes;
        }
        public static List<byte> addQuestionNumber(List<byte> bytes, int i)
        {
            string str = "Q" + Convert.ToString(i);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            bytes = utils.addToByte(bytes, 8, buffer);
            return bytes;
        }
        public static List<byte> addQuestionNumberSubnumber(List<byte> bytes, int question_number, int question_subnumber)
        {
            string str = "Q" + Convert.ToString(question_number) + "." + Convert.ToString(question_subnumber);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            bytes = utils.addToByte(bytes, 8, buffer);
            return bytes;
        }
        public static List<byte> addQuestionNumberSubnumber(List<byte> bytes, int question_number, string question_subtext)
        {
            string str = "Q" + Convert.ToString(question_number) + "." + question_subtext;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            bytes = utils.addToByte(bytes, 8, buffer);
            return bytes;
        }
        public static List<byte> addQuestionLink(List<byte> bytes, int i)
        {

            string q_num = Convert.ToString(i);
            string str = "Q" + q_num + "=Q" + q_num;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            bytes = utils.addToByte(bytes, buffer.Length, buffer);
            return bytes;
        }
        public static List<byte> addQuestionLinkSubnumber(List<byte> bytes, int question_number, int question_subnumber)
        {
            string str = "Q" + Convert.ToString(question_number) + "." + Convert.ToString(question_subnumber) + "=Q" + Convert.ToString(question_number) + "." + Convert.ToString(question_subnumber);
            byte[] buffer1 = System.Text.Encoding.UTF8.GetBytes(str);
            bytes = utils.addToByte(bytes, buffer1.Length, buffer1);
            return bytes;
        }
        public static List<byte> addQuestionLinkSubnumber(List<byte> bytes, int question_number, string question_subtext)
        {
            string str = "Q" + Convert.ToString(question_number) + "." + question_subtext + "=Q" + Convert.ToString(question_number) + "." + question_subtext;
            byte[] buffer1 = System.Text.Encoding.UTF8.GetBytes(str);
            bytes = utils.addToByte(bytes, buffer1.Length, buffer1);
            return bytes;
        }
        public static List<byte> addRoleText(List<byte> bytes)
        {
            bytes.Add(0x3A);
            bytes.Add(0x24);
            bytes.Add(0x40);
            bytes.Add(0x52);
            bytes.Add(0x6F);
            bytes.Add(0x6C);
            bytes.Add(0x65);
            bytes.Add(0x28);
            bytes.Add(0x27);
            bytes.Add(0x30);
            bytes.Add(0x27);
            bytes.Add(0x0A);
            bytes.Add(0x29);
            return bytes;
        }
        public static List<byte> addQuestionRole(List<byte> bytes, int i)
        {

            string q_num = Convert.ToString(i);
            string str = "Q" + q_num;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            bytes = utils.addToByte(bytes, buffer.Length, buffer);
            //and role text
            bytes = utils.addRoleText(bytes);
            return bytes;
        }
        public static List<byte> addQuestionRoleSubnumber(List<byte> bytes, int question_number, int question_subnumber)
        {
            string str = "Q" + Convert.ToString(question_number) + "." + Convert.ToString(question_subnumber);
            byte[] buffer1 = System.Text.Encoding.UTF8.GetBytes(str);
            bytes = utils.addToByte(bytes, buffer1.Length, buffer1);
            bytes = utils.addRoleText(bytes);
            return bytes;
        }
        public static List<byte> addQuestionRoleSubnumber(List<byte> bytes, int question_number, string question_subtext)
        {
            string str = "Q" + Convert.ToString(question_number) + "." + question_subtext;
            byte[] buffer1 = System.Text.Encoding.UTF8.GetBytes(str);
            bytes = utils.addToByte(bytes, buffer1.Length, buffer1);
            bytes = utils.addRoleText(bytes);
            return bytes;
        }
        public static List<byte> addLengthInByte(List<byte> bytes_spss, string text)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(text);
            bytes_spss.Add(Convert.ToByte(buffer.Length));
            return bytes_spss;
        }
        public static List<byte> addMaskedText(List<byte> bytes_spss, string text, int mask)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(text);
            int len;
            if (buffer.Length % mask == 0)
            {
                len = buffer.Length;
            }
            else
            {
                len = ((buffer.Length / mask) + 1) * mask;
            }
            bytes_spss = utils.addToByte(bytes_spss, len, buffer);
            return bytes_spss;
        }
        public static List<byte> addMaskedTextMinusOne(List<byte> bytes_spss, string text, int mask)
        {
            byte[] buffer_full = System.Text.Encoding.UTF8.GetBytes(text);
            if (buffer_full.Length < mask)
            {
                bytes_spss = utils.addToByte(bytes_spss, mask - 1, buffer_full);
            }
            else
            {
                byte[] buffer_begin = new byte[mask-1];
                for (int i = 0; i < mask-1; i++)
                {
                    buffer_begin[i] = buffer_full[i];
                }
                bytes_spss = utils.addToByte(bytes_spss, mask-1, buffer_begin);

                byte[] buffer = new byte[buffer_full.Length - mask + 1];
                for (int i = mask - 1; i< buffer_full.Length; i++)
                {
                    buffer[i - 7] = buffer_full[i];
                }
                int len;
                if (buffer.Length % mask == 0)
                {
                    len = buffer.Length;
                }
                else
                {
                    len = ((buffer.Length / mask) + 1) * mask;
                }
                bytes_spss = utils.addToByte(bytes_spss, len, buffer);
            }
            return bytes_spss;
        }
        public static List<byte> addQuestionHeader(List<byte> bytes_spss, string text)
        {
            text = utils.GetFirstBytes(text,255);
            bytes_spss = utils.addLengthInByte(bytes_spss, text);
            bytes_spss = utils.addGap(bytes_spss);
            bytes_spss = utils.addMaskedText(bytes_spss, text, 4);
            return bytes_spss;
        }
        public static List<byte> addPrefixVariants(List<byte> bytes, int quan)
        {
            bytes.Add(0x03);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(Convert.ToByte(quan));    //quantity of alt
            bytes.Add(0x00);    //quantity of alt?
            bytes.Add(0x00);    //quantity of alt?
            bytes.Add(0x00);    //quantity of alt?

            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0xF0);
            bytes.Add(0x3F);
            return bytes;
        }
        public static List<byte> addBetweenVariants(List<byte> bytes, int num)
        {
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            switch (num)
            {
                case 1:
                    bytes.Add(0x00);
                    break;
                case 2:
                    bytes.Add(0x08);
                    break;
                case 3:
                    bytes.Add(0x10);
                    break;
                case 4:
                    bytes.Add(0x14);
                    break;
                case 5:
                    bytes.Add(0x18);
                    break;
                case 6:
                    bytes.Add(0x1C);
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    bytes.Add(Convert.ToByte(num*2 + 18));
                    break;
                default:
                    bytes.Add(Convert.ToByte(num + 33));
                    break;
            }

            bytes.Add(0x40);
            return bytes;
        }
        public static List<byte> addPostfixVariant(List<byte> bytes, int question_num)
        {
            bytes.Add(0x04);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x01);  //quantity of questions for this variant
            bytes.Add(0x00);  //quantity of questions for this variant
            bytes.Add(0x00);  //quantity of questions for this variant
            bytes.Add(0x00);  //quantity of questions for this variant

            bytes = utils.addBytesOfIntMaskByMask(bytes, question_num, 4);//number of question to (repeated)
            //question number depends of width/8
            return bytes;
        }
        public static List<byte> addPrefixLinks(List<byte> bytes, int quan)
        {
            bytes.Add(0x07);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x03);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x04);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x08);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x1A);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0xD0);
            bytes.Add(0x02);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x01);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x01);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x02);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0xE9);
            bytes.Add(0xFD);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x07);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x04);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x08);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x03);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xEF);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xEF);
            bytes.Add(0x7F);
            bytes.Add(0xFE);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xFF);
            bytes.Add(0xEF);
            bytes.Add(0xFF);
            bytes.Add(0x07);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x0B);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x04);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);


            bytes = utils.addBytesOfIntMaskByMask(bytes, quan*3, 4);//quantity of quest x3

            for (int i = 0; i < quan; i++)
            {
                bytes.Add(0x01);    //repeatable part with parameters of quest
                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0x08);    //colums(width)
                bytes.Add(0x00);    //colums(width)
                bytes.Add(0x00);    //colums(width)
                bytes.Add(0x00);    //colums(width)
                bytes.Add(0x00);    //L-R-justified 0 - L, 1 - R
                bytes.Add(0x00);
                bytes.Add(0x00);
                bytes.Add(0x00);
            }

            bytes.Add(0x07);    //real stable part
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x0D);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x01);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            return bytes;
        }
        public static List<byte> addPrefixRoles(List<byte> bytes, int answers_quan)
        {
            bytes.Add(0x07);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x10);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x08);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x02);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x01);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);

            bytes = addBytesOfIntMaskByMask(bytes,answers_quan,4); //quantity of answers in csv

            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x07);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x12);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x01);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            return bytes;
        }
        public static List<byte> addPostfixRoles(List<byte> bytes)
        {
            bytes.Add(0x07);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x14);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x01);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x05);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x55);
            bytes.Add(0x54);
            bytes.Add(0x46);
            bytes.Add(0x2D);
            bytes.Add(0x38);
            bytes.Add(0xE7);
            bytes.Add(0x03);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            bytes.Add(0x00);
            return bytes;
        }
        public static List<byte> addBytesOfIntMaskByMask(List<byte> bytes, int Value, int mask)
        {
            byte[] buffer = BitConverter.GetBytes(Value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }

            for (int i = 0; i < mask; i++)
            {
                if (i < buffer.Length)
                {
                    bytes.Add(buffer[i]);
                }
                else
                {
                    bytes.Add(0x00);
                }
            }
            return bytes;
        }
        public static List<byte> addPrefixTextAnswer(List<byte> bytes, int len, int max_len)
        {
            for (int i = 0; i < max_len; i++)
            {
                if (i < len)
                {
                    bytes.Add(0xFD);
                }
                else
                {
                    bytes.Add(0xFE);
                }
            }
            return bytes;
        }
        public static List<byte> addPostfixTextAnswer(List<byte> bytes, int len)
        {
            for (int i = 0; i < len; i++)
            {
                bytes.Add(0xFE);
            }
            return bytes;
        }
        public static List<byte> addTextAnswer(List<byte> bytes, List<byte> bytes_string, int max_length)
        {
            int len;
            if (bytes_string.Count % 8 == 0)
            {
                len = bytes_string.Count / 8;
            }
            else
            {
                len = (bytes_string.Count / 8) + 1;
            }
            byte[] buffer = bytes_string.ToArray();
            bytes = addPrefixTextAnswer(bytes, len, max_length);
            len = len * 8;
            bytes = addToByte(bytes, len, buffer);

            return bytes;
        }
        public static List<byte> addAnswer(List<byte> bytes, answer_class answer, int start_length)
        {
            if (answer.isText)
            {
                if (answer.answer != "")
                {
                    answer.answer = GetFirstBytes(answer.answer,200);
                }

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(answer.answer);

                List<byte> part1 = new List<byte>();
                List<byte> part2 = new List<byte>();
                List<byte> part3 = new List<byte>();
                List<byte> part4 = new List<byte>();

                for (int i = 0; i < buffer.Length; i++)
                {
                    if (i < start_length * 8)
                    {
                        part1.Add(buffer[i]);
                    }
                    else if (i < (start_length * 8) + 64)
                    {
                        part2.Add(buffer[i]);
                    }
                    else if (i < (start_length * 8) + 64 * 2)
                    {
                        part3.Add(buffer[i]);
                    }
                    else
                    {
                        part4.Add(buffer[i]);
                    }
                }

                bytes = addTextAnswer(bytes, part1, start_length);
                bytes = addTextAnswer(bytes, part2, 8);
                bytes = addTextAnswer(bytes, part3, 8);
                bytes = addTextAnswer(bytes, part4, 8);

                bytes = addPostfixTextAnswer(bytes,8 - start_length);
            }
            else
            {
                try
                {
                    int i = 100 + Int32.Parse(answer.answer);
                    bytes.Add(Convert.ToByte(i));
                }
                catch
                {
                    bytes.Add(0xFF);
                }
            }
            return bytes;
        }
    }
    class question
    {
        public string text;
        public int type;
        public string[] answers;
        public int num_ans;
        public bool last_text;
    }
    class answer_class
    {
        public string answer;
        public bool isText;
        public int start_length;
    }

    class question_without_commas : question
    {
        public string[] answers_replaced;
    }

    class execute
    {
        private List<answer_class> answers_list = new List<answer_class>();
        private List<question> questions_list = new List<question>();
        private int number_quest;
        private List<byte> bytes_spss = new List<byte>();
        private readonly int max_variants_in_question = 20;
        private bool other_in_main;
        private int answers_quan;

        private int calculate_result_num_questions()
        {
            int question_number = 0;
            //foreach (question questions in questions_list)
            foreach (question questions in questions_list)
            {
                switch (questions.type)
                {
                    case 2:
                    case 7:
                        question_number += 1;

                        if (questions.last_text)
                        {
                            question_number += 1;
                        }
                        break;
                    case 4:
                        for (int j = 0; j < questions.num_ans; j++)
                        {
                            question_number += 1;
                        }
                        if (questions.last_text)
                        {
                            question_number += 1;
                        }
                        break;
                }
            }
            return question_number;
        }
        private List<question_without_commas> construct_questions_without_commas()
        {
            List<question_without_commas> questions_replaced = new List<question_without_commas>();

            foreach(question questions in questions_list)
            {
                question_without_commas question_replaced = new question_without_commas();
                question_replaced.text = questions.text;
                question_replaced.type = questions.type;
                question_replaced.answers = questions.answers;
                question_replaced.answers_replaced = new string[max_variants_in_question];
                if (questions.type == 4)
                {
                    for (int j = 0; j < questions.num_ans; j++)
                    {
                        question_replaced.answers_replaced[j] = questions.answers[j].Replace(";", "");
                        question_replaced.answers_replaced[j] = questions.answers[j].Replace(", ", "");
                    }
                }
                question_replaced.num_ans = questions.num_ans;
                question_replaced.last_text = questions.last_text;
                questions_replaced.Add(question_replaced);
    }
            return questions_replaced;
        }
        private void add_answer_to_list(string answer, bool isText)
        {
            answer_class anwer_obj = new answer_class();
            anwer_obj.isText = isText;
            anwer_obj.answer = answer;
            answers_list.Add(anwer_obj);
        }
        
        public void ask_other()
        {
            while (true)
            {
                Console.WriteLine(@"Add other in main variants (y/n)");
                string response = Console.ReadLine();
                if (response == "y")
                {
                    other_in_main = true;
                    break;
                }
                else if (response == "n")
                {
                    other_in_main = false;
                    break;
                }
             }
        }
        public void read_forms()
        {
            string uri = "";
            Console.WriteLine("Link to Google Form");
            uri = Console.ReadLine();
            string text_html_full = utils.getURI(uri);
            if (text_html_full == "")
            {
                Console.WriteLine("Error in URL");
                return;
            }
            text_html_full = utils.getBetween(text_html_full, "FB_PUBLIC_LOAD_DATA_ = [", "\"/forms\"");
            text_html_full = text_html_full.Replace("\\u0026#160;", " ");
            text_html_full = utils.replaceUnicodeInString(text_html_full);
            text_html_full = System.Net.WebUtility.HtmlDecode(text_html_full);
            text_html_full = utils.cutBegin(text_html_full, "[");
            text_html_full = utils.cutBegin(text_html_full, "[");
            int i = 0;
            //question parse
            while (text_html_full != "")
            {
                string substart = "[";
                string subend = ",[[";
                string quest = utils.getBetween(text_html_full, substart, subend);
                while (utils.numOfSubstringInString(quest,"\"") > 2)
                {
                    quest = utils.cutBegin(quest,"\"");
                }
                string quest_text_begin = utils.getBetween(quest, "\"", "\"");
                //string quest_text_end = quest_text_begin.Replace("\\t", "\\u003cspan style\\u003d\\\"white-space:pre\\\"\\u003e\\t\\u003c/span\\u003e");
                string quest_text_end = quest_text_begin.Replace("\\t", "<span style=\\\"white-space:pre\\\">\\t</span>");
                string text_html = utils.getBetween(text_html_full, quest_text_begin, quest_text_end);
                text_html_full = utils.cutBegin(text_html_full, quest_text_begin, quest_text_end);
                int type = new int();
                try
                {
                    type = Convert.ToInt32(Convert.ToString(quest[quest.Length - 1]));
                }
                catch
                {
                    type = 0;
                }

                if (type == 7)
                {
                    string seven_answers = text_html;
                    string sub_seven = ",[\"";
                    while (seven_answers.Contains(sub_seven))
                    {
                        int j = 0;
                        question questions = new question();
                        questions.type = 2;
                        questions.last_text = false;
                        substart = "[[";
                        subend = "]]";
                        string answers = utils.getBetween(seven_answers, substart, subend);
                        seven_answers = utils.cutBegin(seven_answers, substart, subend);
                        substart = "\"";
                        subend = "\"";
                        questions.answers = new string[11];
                        while (answers.Contains(subend))
                        {
                            if (utils.getBetween(answers, substart, subend) != "")
                            {
                                questions.answers[j] = utils.getBetween(answers, substart, subend);
                            }
                            answers = utils.cutBegin(answers, substart, subend);
                            j++;
                        }
                        questions.num_ans = j;
                        substart = "\"";
                        subend = "\"";
                        questions.text = utils.getBetween(seven_answers, substart, subend);
                        questions.text = questions.text.Replace("\\t", " ");
                        seven_answers = utils.cutBegin(seven_answers, substart, subend);
                        i++;
                        questions_list.Add(questions);
                    }
                }
                else if ((type == 2) || (type == 4))
                {
                    int j = 0;
                    question questions = new question();
                    questions.type = Convert.ToInt32(Convert.ToString(quest[quest.Length - 1]));
                    questions.last_text = false;
                    substart = "\"";
                    subend = "\"";
                    questions.text = utils.getBetween(quest, substart, subend);
                    questions.text = questions.text.Replace("\\t", " ");
                    substart = "[[";
                    subend = "]]";
                    string answers = utils.getBetween(text_html, substart, subend);
                    text_html = utils.cutBegin(text_html, substart, subend);
                    substart = "\"";
                    subend = "]";
                    questions.answers = new string[max_variants_in_question];
                    //variant parse
                    while (answers.Contains(substart))
                    {
                        if (utils.getBetween(answers, substart, substart) != "")
                        {
                            string contains_other = utils.getBetween(answers, substart, subend);
                            if (contains_other != "")
                            {
                                if (contains_other.Substring(contains_other.Length - 4) == "true")
                                {
                                    questions.answers[j] = utils.getBetween(answers, substart, substart);
                                    questions.last_text = true;
                                }
                                else
                                {
                                    questions.answers[j] = utils.getBetween(answers, substart, substart);
                                    j++;
                                }
                            }
                            else
                            {
                                questions.answers[j] = utils.getBetween(answers, substart, substart);
                                j++;
                            }
                        }
                        else
                        {
                            questions.answers[j] = "Другое";
                            if (answers.Substring(answers.Length - 1) == "1")
                            {
                                if (other_in_main)
                                {
                                    questions.last_text = true;
                                    j++;
                                }
                                else
                                {
                                    questions.last_text = true;
                                }
                            }
                            else
                            {
                                j++;
                            }
                        }
                        answers = utils.cutBegin(answers, substart, substart);
                    }
                    questions.num_ans = j;
                    i++;
                    questions_list.Add(questions);
                }
                else
                {
                    text_html_full = "";
                }
            }
            number_quest = i;
        }
        private void leading_bytes()
        {
            bytes_spss.Add(0x24);
            bytes_spss.Add(0x46);
            bytes_spss.Add(0x4C);
            bytes_spss.Add(0x32);
            bytes_spss.Add(0x40);
            bytes_spss.Add(0x28);
            bytes_spss.Add(0x23);
            bytes_spss.Add(0x29);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x49);
            bytes_spss.Add(0x42);
            bytes_spss.Add(0x4D);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x53);
            bytes_spss.Add(0x50);
            bytes_spss.Add(0x53);
            bytes_spss.Add(0x53);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x53);
            bytes_spss.Add(0x54);
            bytes_spss.Add(0x41);
            bytes_spss.Add(0x54);
            bytes_spss.Add(0x49);
            bytes_spss.Add(0x53);
            bytes_spss.Add(0x54);
            bytes_spss.Add(0x49);
            bytes_spss.Add(0x43);
            bytes_spss.Add(0x53);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x36);
            bytes_spss.Add(0x34);
            bytes_spss.Add(0x2D);
            bytes_spss.Add(0x62);
            bytes_spss.Add(0x69);
            bytes_spss.Add(0x74);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x4D);
            bytes_spss.Add(0x53);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x57);
            bytes_spss.Add(0x69);
            bytes_spss.Add(0x6E);
            bytes_spss.Add(0x64);
            bytes_spss.Add(0x6F);
            bytes_spss.Add(0x77);
            bytes_spss.Add(0x73);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x32);
            bytes_spss.Add(0x36);
            bytes_spss.Add(0x2E);
            bytes_spss.Add(0x30);
            bytes_spss.Add(0x2E);
            bytes_spss.Add(0x30);
            bytes_spss.Add(0x2E);
            bytes_spss.Add(0x30);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x02);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);  // bytes of lenght
            bytes_spss.Add(0x00);  // bytes of lenght
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x01);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x59);
            bytes_spss.Add(0x40);
            bytes_spss.Add(0x31);
            bytes_spss.Add(0x32);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x41);
            bytes_spss.Add(0x70);
            bytes_spss.Add(0x72);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x32);
            bytes_spss.Add(0x32);
            bytes_spss.Add(0x31);
            bytes_spss.Add(0x33);
            bytes_spss.Add(0x3A);
            bytes_spss.Add(0x35);
            bytes_spss.Add(0x34);
            bytes_spss.Add(0x3A);
            bytes_spss.Add(0x30);
            bytes_spss.Add(0x38);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x20);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
            bytes_spss.Add(0x00);
        }
        private void question_declarations()
        {

            int question_number = 0;
            foreach (question questions in questions_list)
            {
                switch (questions.type)
                {
                    case 2:
                        bytes_spss = utils.addPrefixQuestion(bytes_spss);
                        question_number += 1;
                        bytes_spss = utils.addQuestionNumber(bytes_spss, question_number);
                        bytes_spss = utils.addQuestionHeader(bytes_spss, questions.text);

                        if (questions.last_text)
                        {
                            bytes_spss = utils.addPrefixOther(bytes_spss);
                            bytes_spss = utils.addQuestionNumberSubnumber(bytes_spss, question_number, "T");
                            bytes_spss = utils.addQuestionHeader(bytes_spss, questions.text);
                            bytes_spss = utils.addPostfixOther(bytes_spss);
                        }
                        break;
                    case 4:
                        question_number += 1;
                        int question_subnumber = 0;
                        for (int j = 0; j < questions.num_ans; j++)
                        {
                            bytes_spss = utils.addPrefixQuestion(bytes_spss);
                            question_subnumber += 1;
                            bytes_spss = utils.addQuestionNumberSubnumber(bytes_spss, question_number, question_subnumber);
                            bytes_spss = utils.addQuestionHeader(bytes_spss, questions.answers[j]);
                        }
                        if (questions.last_text)
                        {
                            bytes_spss = utils.addPrefixOther(bytes_spss);
                            bytes_spss = utils.addQuestionNumberSubnumber(bytes_spss, question_number, "T");
                            if (other_in_main)
                            {
                                bytes_spss = utils.addQuestionHeader(bytes_spss, questions.answers[questions.num_ans - 1]);
                            }
                            else
                            {
                                bytes_spss = utils.addQuestionHeader(bytes_spss, questions.answers[questions.num_ans]);
                            }
                            bytes_spss = utils.addPostfixOther(bytes_spss);
                        }
                        break;
                }
            }
        }
        private void add_variants()
        {
            int question_number = 0;
            foreach (question questions in questions_list)
            {
                switch (questions.type)
                {
                    case 2:
                        question_number += 1;
                        bytes_spss = utils.addPrefixVariants(bytes_spss, questions.num_ans);
                        for (int j = 0; j < questions.num_ans; j++)
                        {
                            string text = utils.GetFirstBytes(questions.answers[j],255);
                            bytes_spss = utils.addLengthInByte(bytes_spss, text);
                            bytes_spss = utils.addMaskedTextMinusOne(bytes_spss, text, 8);
                            if (j == questions.num_ans - 1)
                            {
                                bytes_spss = utils.addPostfixVariant(bytes_spss, question_number);
                            }
                            else
                            {
                                bytes_spss = utils.addBetweenVariants(bytes_spss, j+1);
                            }
                        }

                        if (questions.last_text)
                        {
                            question_number += 32;
                        }
                        break;
                    case 4:
                        for (int j = 0; j < questions.num_ans; j++)
                        {
                            question_number += 1;
                        }
                        if (questions.last_text)
                        {
                            question_number += 32;
                        }
                        break;
                }
            }
        }
        private void addLinks()
        {
            bytes_spss = utils.addPrefixLinks(bytes_spss, calculate_result_num_questions());
            List<byte> bytes_links = new List<byte>();
            int question_number = 0;
            int i = -1;
            foreach (question questions in questions_list)
            {
                i++;
                switch (questions.type)
                {
                    case 2:
                        question_number += 1;
                        bytes_links = utils.addQuestionLink(bytes_links, question_number);

                        if (questions.last_text)
                        {
                            bytes_links.Add(0x09);
                            bytes_links = utils.addQuestionLinkSubnumber(bytes_links, question_number, "T");
                        }
                        if (!(i == number_quest - 1))
                        {
                            bytes_links.Add(0x09);
                        }
                        break;
                    case 4:
                        question_number += 1;
                        int question_subnumber = 0;
                        for (int j = 0; j < questions.num_ans; j++)
                        {
                            question_subnumber += 1;
                            bytes_links = utils.addQuestionLinkSubnumber(bytes_links, question_number, question_subnumber);

                            if (!((i == number_quest - 1) && (j == questions.num_ans - 1) && (!questions.last_text)))
                            {
                                bytes_links.Add(0x09);
                            }
                        }
                        if (questions.last_text)
                        {
                            bytes_links = utils.addQuestionLinkSubnumber(bytes_links, question_number, "T");
                            if (!(i == number_quest - 1))
                            {
                                bytes_links.Add(0x09);
                            }
                        }
                        break;
                }
            }
            byte[] buffer = bytes_links.ToArray();
            bytes_spss = utils.addBytesOfIntMaskByMask(bytes_spss, buffer.Length, 4);
            bytes_spss.AddRange(bytes_links);
        }
        private void addRoles()
        {
            bytes_spss = utils.addPrefixRoles(bytes_spss, answers_quan);
            List<byte> bytes_roles = new List<byte>();
            int question_number = 0;
            int i = -1;
            foreach (question questions in questions_list)
            {
                i++;
                switch (questions.type)
                {
                    case 2:
                        question_number += 1;
                        bytes_roles = utils.addQuestionRole(bytes_roles, question_number);

                        if (questions.last_text)
                        {
                            bytes_roles.Add(0x2F);
                            bytes_roles = utils.addQuestionRoleSubnumber(bytes_roles, question_number, "T");
                        }
                        if (i != number_quest - 1)
                        {
                            bytes_roles.Add(0x2F);
                        }
                        break;
                    case 4:
                        question_number += 1;
                        int question_subnumber = 0;
                        for (int j = 0; j < questions.num_ans; j++)
                        {
                            question_subnumber += 1;
                            bytes_roles = utils.addQuestionRoleSubnumber(bytes_roles, question_number, question_subnumber);

                            if (!((i == number_quest - 1) && (j == questions.num_ans - 1) && (!questions.last_text)))
                            {
                                bytes_roles.Add(0x2F);
                            }
                        }
                        if (questions.last_text)
                        {
                            bytes_roles = utils.addQuestionRoleSubnumber(bytes_roles, question_number, "T");
                            if (!(i == number_quest - 1))
                            {
                                bytes_roles.Add(0x2F);
                            }
                        }
                        break;
                }
            }
            byte[] buffer = bytes_roles.ToArray();
            bytes_spss = utils.addBytesOfIntMaskByMask(bytes_spss, buffer.Length, 4);
            bytes_spss.AddRange(bytes_roles);
            bytes_spss = utils.addPostfixRoles(bytes_spss);
        }
        private void addData()
        {
            List<byte> bytes = new List<byte>();
            int start_length = 8;
            foreach(answer_class answer in answers_list)
            {
                if (!answer.isText)
                {
                    start_length = start_length - 1;
                }
                if (start_length == 0)
                {
                    start_length = 8;
                }
                bytes = utils.addAnswer(bytes, answer, start_length);
            }

            long additional_zeroes = 8 - bytes.Count % 8;
            if (additional_zeroes == 8)
            {
                additional_zeroes = 0;
            }
            for (int i = 0; i < additional_zeroes; i++)
            {
                bytes.Add(0x00);
            }
            bytes_spss.AddRange(bytes);
        }
        private List<byte> addDataThread(List<answer_class> answers_list)
        {
            List<byte> bytes = new List<byte>();
            foreach (answer_class answer in answers_list)
            {
                bytes = utils.addAnswer(bytes, answer, answer.start_length);
            }
            return bytes;
        }
        private void addDataWithThreading()
        {
            int thread_number = 4;
            List<byte>[] bytes = new List<byte> [thread_number];
            Task[] AllTasks = new Task[thread_number];

            int start_length = 8;
            foreach (answer_class answer in answers_list)
            {
                if (!answer.isText)
                {
                    start_length = start_length - 1;
                }
                if (start_length == 0)
                {
                    start_length = 8;
                }
                answer.start_length = start_length;
            }

            List<answer_class>[] answers_list_thread = new List<answer_class> [thread_number];

            //for (int i = 0; i < thread_number - 1; i++)
            //{
            //    int j = i;
            //    AllTasks[i] = new Task(() => answers_list_thread[j] = answers_list.GetRange(j*(answers_list.Count / thread_number), answers_list.Count / thread_number));
            //}
            //AllTasks[thread_number - 1] = new Task(() => answers_list_thread[(thread_number - 1)] 
            //    = answers_list.GetRange((thread_number-1) * (answers_list.Count / thread_number), answers_list.Count - ((thread_number - 1) * (answers_list.Count / thread_number))));

            //foreach (Task task in AllTasks)
            //{
            //    task.Start();
            //}
            //Task.WaitAll(AllTasks);

            for (int i = 0; i < thread_number - 1; i++)
            {
                answers_list_thread[i] = answers_list.GetRange(i * (answers_list.Count / thread_number), answers_list.Count / thread_number);
            }
            answers_list_thread[(thread_number - 1)]
                = answers_list.GetRange((thread_number - 1) * (answers_list.Count / thread_number), answers_list.Count - ((thread_number - 1) * (answers_list.Count / thread_number)));
            //^

            for (int i = 0; i < thread_number; i++)
            {
                int j = i;
                AllTasks[i] = new Task(() => bytes[j] = addDataThread(answers_list_thread[j]));
            }
            foreach (Task task in AllTasks)
            {
                task.Start();
            }

            //int additional_zeroes = 0;
            //for (int task_num = 0; task_num < thread_number; task_num++)
            //{
            //    AllTasks[task_num].Wait();
            //    bytes_spss.AddRange(bytes[task_num]);
            //    additional_zeroes = additional_zeroes + bytes[task_num].Count;
            //}

            Task.WaitAll(AllTasks);

            long additional_zeroes = 0;
            for (int i = 0; i < thread_number; i++)
            {
                bytes_spss.AddRange(bytes[i]);
                additional_zeroes = additional_zeroes + bytes[i].Count;
            }
            //^

            additional_zeroes = 8 - (additional_zeroes % 8);
            if (additional_zeroes == 8)
            {
                additional_zeroes = 0;
            }
            for (int i = 0; i < additional_zeroes; i++)
            {
                bytes_spss.Add(0x00);
            }
        }
        public void create_spss()
        {
            //creation of spss statistics file
            read_csv();
            leading_bytes();
            question_declarations();
            add_variants();
            addLinks();
            addRoles();
            if (answers_list.Count > 0)
            {
                addData();
            }

            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            //addDataWithThreading();
            //stopWatch.Stop();
            //// Get the elapsed time as a TimeSpan value.
            //TimeSpan ts = stopWatch.Elapsed;

            //// Format and display the TimeSpan value.
            //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    ts.Hours, ts.Minutes, ts.Seconds,
            //    ts.Milliseconds / 10);
            //Console.WriteLine("RunTime " + elapsedTime);
        }
        public void save_spss()
        {
            string filePath = Directory.GetCurrentDirectory();
            filePath = filePath + @"\result.sav";
            byte[] buffer = bytes_spss.ToArray();
            try
            {
                //if (!File.Exists((@"C:\result.sav")))
                //{
                //    File.Create((@"C:\result.sav"));
                //}
                File.WriteAllBytes(filePath, buffer);
            }
            catch
            {
                Console.WriteLine("SPSS file access error");
            }
            bytes_spss = null;
        }
        public void save_csv()
        {
            List<question_without_commas> questions_replaced = construct_questions_without_commas();
            Console.WriteLine("Answers csv file");
            string path = Console.ReadLine();
            string[] lines;
            try
            {
                lines = File.ReadAllLines(path, Encoding.Default);
            }
            catch
            {
                Console.WriteLine("Error in csv-filepath");
                return;
            }

            string[] csv_lines = new string[lines.Length - 1];

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = utils.cutBegin(lines[i], ";");
                lines[i] = lines[i] + ";";
            }
            for (int k = 1; k < lines.Length; k++)
            {
                foreach (question_without_commas questions in questions_replaced)
                {
                    switch (questions.type)
                    {
                        case 2:
                            string answer = utils.getBetween(lines[k], ";");
                            lines[k] = utils.cutBegin(lines[k], ";");
                            if (answer != "")
                            {
                                bool flag_other = true;
                                for (int j = 0; j < questions.num_ans; j++)
                                {
                                    questions.answers[j] = questions.answers[j].Replace(";", "");
                                    if (answer == questions.answers[j])
                                    {
                                        if (other_in_main)
                                        {
                                            if (!(questions.last_text && (j + 1 == questions.num_ans)))
                                            {
                                                csv_lines[k - 1] = csv_lines[k - 1] + Convert.ToString(j + 1) + ";";
                                                flag_other = false;
                                            }
                                        }
                                        else
                                        {
                                            csv_lines[k - 1] = csv_lines[k - 1] + Convert.ToString(j + 1) + ";";
                                            flag_other = false;
                                        }
                                    }
                                }
                                if (questions.last_text)
                                {
                                    if (flag_other)
                                    {
                                        if (other_in_main)
                                        {
                                            csv_lines[k - 1] = csv_lines[k - 1] + Convert.ToString(questions.num_ans) + ";" + answer + ";";
                                        }
                                        else
                                        {
                                            csv_lines[k - 1] = csv_lines[k - 1] + ";" + answer + ";";
                                        }

                                    }
                                    else
                                    {
                                        csv_lines[k - 1] = csv_lines[k - 1] + ";";
                                    }
                                }
                            }
                            else
                            {
                                csv_lines[k - 1] = csv_lines[k - 1] + ";";
                                if (questions.last_text)
                                {
                                    csv_lines[k - 1] = csv_lines[k - 1] + ";";
                                }
                            }
                            break;
                        case 4:

                            int num_ans;
                            if (other_in_main)
                            {
                                num_ans = questions.num_ans - 1;
                            }
                            else
                            {
                                num_ans = questions.num_ans;
                            }
                            bool[] question_yes = new bool[num_ans];
                            for (int j = 0; j < question_yes.Length; j++)
                            {
                                question_yes[j] = false;
                            }
                            string answer_full = utils.getBetween(lines[k], ";");
                            lines[k] = utils.cutBegin(lines[k], ";");
                            for (int j = 0; j < questions.num_ans; j++)
                            {
                                answer_full = answer_full.Replace(questions.answers[j], questions.answers_replaced[j]);
                            }
                            for (int j = 0; j < num_ans; j++)
                            {
                                string answer_single = utils.getBetween(answer_full, ", ");
                                if (answer_single == "")
                                {
                                    answer_single = answer_full;
                                }
                                for (int l = j; l < num_ans; l++)
                                {
                                    if (questions.answers_replaced[l] == answer_single)
                                    {
                                        question_yes[l] = true;
                                        j = l;
                                        answer_full = utils.cutBegin(answer_full, ", ");
                                    }
                                }
                            }
                            for (int j = 0; j < num_ans; j++)
                            {
                                if (question_yes[j])
                                {
                                    csv_lines[k - 1] = csv_lines[k - 1] + "1;";
                                }
                                else
                                {
                                    csv_lines[k - 1] = csv_lines[k - 1] + ";";
                                }
                            }
                            if (questions.last_text)
                            {
                                if (other_in_main)
                                {
                                    if (answer_full != "")
                                    {
                                        csv_lines[k - 1] = csv_lines[k - 1] + "1;" + answer_full + ";";
                                    }
                                    else
                                    {
                                        csv_lines[k - 1] = csv_lines[k - 1] + ";" + answer_full + ";";
                                    }
                                }
                                else
                                {
                                    csv_lines[k - 1] = csv_lines[k - 1] + answer_full + ";";
                                }
                            }
                            break;
                    }
                }
            }
            try
            {
                string filePath = Directory.GetCurrentDirectory();
                filePath = filePath + @"\result.csv";
                File.WriteAllLines(filePath, csv_lines, Encoding.Default);
            }
            catch
            {
                Console.WriteLine("CSV file access error");
            }
        }
        public void read_csv()
        {
            List<question_without_commas> questions_replaced = construct_questions_without_commas();
            Console.WriteLine("Answers csv file");
            string path = Console.ReadLine();
            string[] lines;
            try
            {
                lines = File.ReadAllLines(path, Encoding.Default);
            }
            catch
            {
                Console.WriteLine("Error in csv-filepath");
                return;
            }

            //string[] csv_lines = new string[lines.Length - 1];

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = utils.cutBegin(lines[i], ";");
                lines[i] = lines[i] + ";";
            }
            answers_quan = lines.Length - 1;
            for (int k = 1; k < lines.Length; k++)
            {
                foreach (question_without_commas questions in questions_replaced)
                {
                    switch (questions.type)
                    {
                        case 2:
                            string answer = utils.getBetween(lines[k], ";");
                            lines[k] = utils.cutBegin(lines[k], ";");
                            if (answer != "")
                            {
                                bool flag_other = true;
                                for (int j = 0; j < questions.num_ans; j++)
                                {
                                    questions.answers[j] = questions.answers[j].Replace(";", "");
                                    if (answer == questions.answers[j])
                                    {
                                        if (other_in_main)
                                        {
                                            if (!(questions.last_text && (j + 1 == questions.num_ans)))
                                            {
                                                add_answer_to_list(Convert.ToString(j + 1), false);
                                                //csv_lines[k - 1] = csv_lines[k - 1] + Convert.ToString(j + 1) + ";";
                                                flag_other = false;
                                            }
                                        }
                                        else
                                        {
                                            add_answer_to_list(Convert.ToString(j + 1), false);
                                            //csv_lines[k - 1] = csv_lines[k - 1] + Convert.ToString(j + 1) + ";";
                                            flag_other = false;
                                        }
                                    }
                                }
                                if (questions.last_text)
                                {
                                    if (flag_other)
                                    {
                                        if (other_in_main)
                                        {
                                            add_answer_to_list(Convert.ToString(questions.num_ans), false);
                                            add_answer_to_list(answer, true);
                                            //csv_lines[k - 1] = csv_lines[k - 1] + Convert.ToString(questions.num_ans) + ";" + answer + ";";
                                        }
                                        else
                                        {
                                            add_answer_to_list("", false);
                                            add_answer_to_list(answer, true);
                                            //csv_lines[k - 1] = csv_lines[k - 1] + ";" + answer + ";";
                                        }

                                    }
                                    else
                                    {
                                        add_answer_to_list("", true);
                                        //csv_lines[k - 1] = csv_lines[k - 1] + ";";
                                    }
                                }
                            }
                            else
                            {
                                add_answer_to_list("", false);
                                //csv_lines[k - 1] = csv_lines[k - 1] + ";";
                                if (questions.last_text)
                                {
                                    add_answer_to_list("", true);
                                    //csv_lines[k - 1] = csv_lines[k - 1] + ";";
                                }
                            }
                            break;
                        case 4:

                            int num_ans;
                            if (other_in_main)
                            {
                                num_ans = questions.num_ans - 1;
                            }
                            else
                            {
                                num_ans = questions.num_ans;
                            }
                            bool[] question_yes = new bool[num_ans];
                            for (int j = 0; j < question_yes.Length; j++)
                            {
                                question_yes[j] = false;
                            }
                            string answer_full = utils.getBetween(lines[k], ";");
                            lines[k] = utils.cutBegin(lines[k], ";");
                            for (int j = 0; j < questions.num_ans; j++)
                            {
                                answer_full = answer_full.Replace(questions.answers[j], questions.answers_replaced[j]);
                            }
                            for (int j = 0; j < num_ans; j++)
                            {
                                string answer_single = utils.getBetween(answer_full, ", ");
                                if (answer_single == "")
                                {
                                    answer_single = answer_full;
                                }
                                for (int l = j; l < num_ans; l++)
                                {
                                    if (questions.answers_replaced[l] == answer_single)
                                    {
                                        question_yes[l] = true;
                                        j = l;
                                        answer_full = utils.cutBegin(answer_full, ", ");
                                    }
                                }
                            }
                            for (int j = 0; j < num_ans; j++)
                            {
                                if (question_yes[j])
                                {
                                    add_answer_to_list("1", false);
                                    //csv_lines[k - 1] = csv_lines[k - 1] + "1;";
                                }
                                else
                                {
                                    add_answer_to_list("", false);
                                    //csv_lines[k - 1] = csv_lines[k - 1] + ";";
                                }
                            }
                            if (questions.last_text)
                            {
                                if (other_in_main)
                                {
                                    if (answer_full != "")
                                    {
                                        add_answer_to_list("1", false);
                                        add_answer_to_list(answer_full, true);
                                        //csv_lines[k - 1] = csv_lines[k - 1] + "1;" + answer_full + ";";
                                    }
                                    else
                                    {
                                        add_answer_to_list("", false);
                                        add_answer_to_list(answer_full, true);
                                        //csv_lines[k - 1] = csv_lines[k - 1] + ";" + answer_full + ";";
                                    }
                                }
                                else
                                {
                                    add_answer_to_list(answer_full, true);
                                    //csv_lines[k - 1] = csv_lines[k - 1] + answer_full + ";";
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
    
    class Program
    {
        
        static void Main(string[] args)
        {
            execute exec = new execute();

            exec.ask_other();
            exec.read_forms();
            exec.create_spss();
            exec.save_spss();
            Console.Write("done");
            Console.ReadKey();
        }
    }
}
