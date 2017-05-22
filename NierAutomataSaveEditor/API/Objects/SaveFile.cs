using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NierAutomataSaveEditor.API.Objects
{
    public class SaveFile
    {
        #region Variables
        public int Money;
        public int EXP;
        public Dictionary<short, short> Items;
        #endregion

        #region Properties
        public string Path { get; private set; }
        public byte[] Bytes { get; private set; }

        public string Name
        {
            get
            {
                return Encoding.Unicode.GetString(Bytes.SubArray((int)Offsets.NAME_START, (((int)Offsets.NAME_END - (int)Offsets.NAME_START) + 1)));
            }
        }
        #endregion

        public SaveFile(string file)
        {
            this.Path = file;
            this.Bytes = File.ReadAllBytes(Path);

            Money = BitConverter.ToInt32(Bytes.SubArray((int)Offsets.MONEY_START, (((int)Offsets.MONEY_END - (int)Offsets.MONEY_START) + 1)), 0);
            EXP = BitConverter.ToInt32(Bytes.SubArray((int)Offsets.EXP_START, (((int)Offsets.EXP_END - (int)Offsets.EXP_START) + 1)), 0);
            Items = new Dictionary<short, short>();

            int pos = (int)Offsets.ITEM_START;
            while ((pos + 12) <= (int)Offsets.ITEM_END)
            {
                short ID = BitConverter.ToInt16(Bytes.SubArray(pos, 2), 0);
                short Count = BitConverter.ToInt16(Bytes.SubArray((pos + 8), 2), 0);

                pos += 12;
                if (ID == -1)
                    continue;

                Items.Add(ID, Count);
            }
        }

        #region Public Functions
        public bool Save()
        {
            try
            {
                byte[] AMoney = BitConverter.GetBytes(Money);
                byte[] AEXP = BitConverter.GetBytes(EXP);
                byte[] AEnd = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                int pos = (int)Offsets.ITEM_START;

                Bytes.Modify((int)Offsets.MONEY_START, AMoney);
                Bytes.Modify((int)Offsets.EXP_START, AEXP);
                foreach(KeyValuePair<short, short> item in Items)
                {
                    if ((pos + 12) > (int)Offsets.ITEM_END)
                        break;

                    byte[] BItem = new byte[12];
                    byte[] BID = BitConverter.GetBytes(item.Key);
                    byte[] BAmount = BitConverter.GetBytes(item.Value);
                    byte[] BSig = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x07 };
                    byte[] BEnd = new byte[] { 0x00, 0x00, 0x00 };

                    if (BAmount[1] == 0x00)
                        Array.Reverse(BAmount);

                    BItem.Modify(0, BID);
                    BItem.Modify(2, BSig);
                    BItem.Modify(7, BAmount);
                    BItem.Modify(9, BEnd);
                    Bytes.Modify(pos, BItem);

                    pos += BItem.Length;
                }
                Bytes.Modify(pos, AEnd);
                pos += AEnd.Length;
                while ((pos + 12) <= (int)Offsets.ITEM_END)
                {
                    byte[] BItem = new byte[12];
                    byte[] BEmpty = new byte[] { 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF };
                    byte[] BEnd = new byte[] { 0xFF, 0xFF, 0xFF };

                    BItem.Modify(0, BEmpty);
                    BItem.Modify(9, BEnd);
                    Bytes.Modify(pos, BItem);

                    pos += BItem.Length;
                }

                File.WriteAllBytes(Path, Bytes);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
        #endregion
    }
}
