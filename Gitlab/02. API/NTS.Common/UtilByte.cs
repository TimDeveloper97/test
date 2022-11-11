using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.Common
{
    public static class UtilByte
    {
        /// <summary>
        /// chuyển byte ra int theo đoạn
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int ConvertByteToInt(byte[] input, int start, int n)
        {
            byte[] byted = getbyte(input, start, n);
            if (byted.Length == 1)
            {
                return Convert.ToInt32((sbyte)byted[0]);
            }
            else if (n == 2)
            {
                return (Int32)(BitConverter.ToInt16(byted, 0));
            }
            else if (n == 3)
            {
                byte[] Byte = new byte[4];
                Array.Copy(byted, 0, Byte, 1, 3);
                return BitConverter.ToInt32(Byte, 0);
            }
            else if (n == 4)
            {
                return BitConverter.ToInt32(byted, 0);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// chuyển string dạng "0xXXXXXXXXX" thành mảng byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ConvertToByteArray(string value)
        {
            byte[] bytes = null;
            if (String.IsNullOrEmpty(value))
                bytes = null;
            else
            {
                int string_length = value.Length;
                int character_index = (value.StartsWith("0x", StringComparison.Ordinal)) ? 2 : 0; // Does the string define leading HEX indicator '0x'. Adjust starting index accordingly.               
                int number_of_characters = string_length - character_index;

                bool add_leading_zero = false;
                if (0 != (number_of_characters % 2))
                {
                    add_leading_zero = true;

                    number_of_characters += 1;  // Leading '0' has been striped from the string presentation.
                }

                bytes = new byte[number_of_characters / 2]; // Initialize our byte array to hold the converted string.

                int write_index = 0;
                if (add_leading_zero)
                {
                    bytes[write_index++] = FromCharacterToByte(value[character_index], character_index);
                    character_index += 1;
                }

                for (int read_index = character_index; read_index < value.Length; read_index += 2)
                {
                    byte upper = FromCharacterToByte(value[read_index], read_index, 4);
                    byte lower = FromCharacterToByte(value[read_index + 1], read_index + 1);

                    bytes[write_index++] = (byte)(upper | lower);
                }
            }

            return bytes;
        }
        private static byte FromCharacterToByte(char character, int index, int shift = 0)
        {
            byte value = (byte)character;
            if (((0x40 < value) && (0x47 > value)) || ((0x60 < value) && (0x67 > value)))
            {
                if (0x40 == (0x40 & value))
                {
                    if (0x20 == (0x20 & value))
                        value = (byte)(((value + 0xA) - 0x61) << shift);
                    else
                        value = (byte)(((value + 0xA) - 0x41) << shift);
                }
            }
            else if ((0x29 < value) && (0x40 > value))
                value = (byte)((value - 0x30) << shift);
            else
                throw new InvalidOperationException(String.Format("Character '{0}' at index '{1}' is not valid alphanumeric character.", character, index));

            return value;
        }

        public static byte[] getbyte(byte[] input, int start, int n)
        {
            byte[] Byte = new byte[n];
            Array.Copy(input, start, Byte, 0, n);
            return Byte;
        }

        /// <summary>
        /// tìm mảng byte
        /// </summary>
        /// <param name="src">đầu vào</param>
        /// <param name="find">giá trị cần tìm</param>
        /// <returns></returns>
        private static int FindBytes(byte[] src, byte[] find)
        {
            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }

            }
            return index;
        }
        /// <summary>
        /// thay thế byte
        /// </summary>
        /// <param name="src"></param>
        /// <param name="search"></param>
        /// <param name="repl"></param>
        /// <returns></returns>
        public static byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl)
        {
            byte[] dst = null;
            int index = FindBytes(src, search);
            if (index >= 0)
            {
                dst = new byte[src.Length - search.Length + repl.Length];
                // before found array
                Buffer.BlockCopy(src, 0, dst, 0, index);
                // repl copy
                Buffer.BlockCopy(repl, 0, dst, index, repl.Length);
                // rest of src array
                Buffer.BlockCopy(
                    src,
                    index + search.Length,
                    dst,
                    index + repl.Length,
                    src.Length - (index + search.Length));

                return ReplaceBytes(dst, search, repl);

            }
            else
            {
                return src;
            }

        }
        /// <summary>
        /// cắt bỏ byte
        /// </summary>
        /// <param name="src"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="retf"></param>
        /// <returns></returns>
        public static List<byte[]> SplitBytes(byte[] src, byte[] start, byte[] end, List<byte[]> retf)
        {
            int indexstart = FindBytes(src, start);
            int indexend = FindBytes(src, end);
            if (src.Length > 0 && indexstart >= 0 && indexend >= 0 && indexstart < indexend)
            {
                retf.Add(getbyte(src, indexstart, indexend + 3 - indexstart));

                byte[] Bytes = new byte[src.Length - (indexend + 3 - indexstart)];
                Array.Copy(src, indexend + 3 - indexstart, Bytes, 0, src.Length - (indexend + 3 - indexstart));

                return SplitBytes(Bytes, start, end, retf);
            }
            else
            {
                return retf;
            }

        }
        /// <summary>
        /// chuyển mảng byte thành string hex
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] src)
        {
            StringBuilder hex = new StringBuilder(src.Length * 2);

            foreach (byte b in src)
                hex.AppendFormat("0x{0:x2}-", b);
            return hex.ToString();
        }
        /// <summary>
        /// chuyển từng byte thành từng ký tự char
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string ByteArrayToCharString(byte[] src)
        {
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < src.Length; i++)
            {
                strBuilder.Append((char)src[i]);
            }
            return strBuilder.ToString();
        }
          
    }
}
