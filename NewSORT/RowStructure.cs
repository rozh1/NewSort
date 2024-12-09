using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NewSORT
{
    class RowStructure : IComparable
    {
        public RowStructure(string beforeData, int sortingColumn, string afterData) {
            
            SortingColumn = sortingColumn; 
            AfterData = afterData;
            BeforeData = beforeData;
            
            }   
        public int SortingColumn
        {
            get; set;
        }

        public string BeforeData
        {
            get; set;
        }

        public string AfterData
        {
            get; set;
        }

        public int CompareTo(object obj)
        {
            if(obj is RowStructure person) return SortingColumn.CompareTo(person.SortingColumn);
        else throw new ArgumentException("Некорректное значение параметра");
        }

        public string GetRow()
        {
            if (string.IsNullOrEmpty(BeforeData)){
                return string.Format("\"{0}\"|{1}", SortingColumn, AfterData);}
            if (string.IsNullOrEmpty(AfterData)){
                return string.Format("{0}\"{1}\"", BeforeData, SortingColumn);}
            return string.Format("{0}\"{1}\"|{2}", BeforeData, SortingColumn, AfterData);
            
        }


    }
}
