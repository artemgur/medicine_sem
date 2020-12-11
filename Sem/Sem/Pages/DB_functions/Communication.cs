using System;

namespace Sem.Сommunication
{
    public class Communication
    {
        private string nameTable;
        private int id1;
        private int id2;

        private string nameId1;
        private string nameId2;

        public void SetId(string nameTable, int id1, int id2, string nameId1, string nameId2)
        {
            this.nameTable = nameTable;
            this.nameId1 = nameId1;
            this.nameId2 = nameId2;
            this.id1 = id1;
            this.id2 = id2;
        }


        public void Add()
        {
            try
            {
                DataBase.Add("INSERT INTO " + nameTable + " VALUES (" + id1 + ", " + id2 + ");");
            }
            catch
            {
                throw new Exception("Invalid data was passed when calling the command!");
            }
        }

        public void Remove()
        {
            try
            {
                DataBase.Add("DELETE FROM " + nameTable + " WHERE " + nameId1 + " = " + id1 + " AND " + nameId2 + " = " + id2 + ";");
            }
            catch
            {
                throw new Exception("Invalid data was passed when calling the command!");
            }
        }

        public int CountComs()
        {
            return DataBase.Select("SELECT * FROM " + 
                nameTable + 
                " WHERE " + nameId1 + " = " + id1 + "AND " + nameId2 + " = " + id2 + ";").Count;
        }
    }
}
