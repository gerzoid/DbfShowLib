using DbfShowLib.Sorting.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbfShowLib.Sorting
{
    class Sort
    {
        private string columnNameSort;
        private SortingType activeSortingType;

        public SortingType GetActiveSortingType
        {
            get { return activeSortingType; }
        }

        public string GetColumnNameSort
        {
            get { return columnNameSort; }
        }


        public void SortArray(ref string[] value, ref int[] keys, string ColumnType, SortingType sortingType, int start, int length)
        {
            //ColumnType = "DATE";
            if (sortingType == SortingType.ASC)
            {
                if ((ColumnType == "DATE") || (ColumnType == "DATETIME"))           //Если сортируемая колонка имеет тип Date илиDateTime
                    Array.Sort(value, keys, start, length, new DateComparerAsc());          //То используем сравнение со сцециальным DateComparer
                else
                    if ((ColumnType == "NUMERIC") || (ColumnType == "FLOAT") || (ColumnType == "CURRENCY") || (ColumnType == "INTEGER") || (ColumnType == "DOUBLE"))
                    Array.Sort(value, keys, start, length, new NumericComparerAsc());      //Используем стандартную сортировку
                else
                    Array.Sort(value, keys, start, length, new StringComparerAsc());      //Используем стандартную сортировку
            }
            else
                if (sortingType == SortingType.DESC)
            {
                if ((ColumnType == "DATE") || (ColumnType == "DATETIME"))           //Если сортируемая колонка имеет тип Date илиDateTime
                    Array.Sort(value, keys, start, length, new DateComparerDesc());          //То используем сравнение со сцециальным DateComparer
                else
                    if ((ColumnType == "NUMERIC") || (ColumnType == "FLOAT") || (ColumnType == "CURRENCY") || (ColumnType == "INTEGER") || (ColumnType == "DOUBLE"))
                    Array.Sort(value, keys, start, length, new NumericComparerDesc());      //Используем стандартную сортировку
                else
                    Array.Sort(value, keys, start, length, new StringComparerDesc());      //Используем стандартную сортировку
            }
            activeSortingType = sortingType;
            //columnNameSort = ColumnName;
        }



        public void SortArray(ref string[] value, ref int[] keys, string ColumnName, string ColumnType)
        {
            //ColumnType = "DATE";
            if (columnNameSort != ColumnName)
            {
                if ((ColumnType == "DATE") || (ColumnType == "DATETIME"))           //Если сортируемая колонка имеет тип Date илиDateTime
                    Array.Sort(value, keys, new DateComparerAsc());          //То используем сравнение со сцециальным DateComparer
                else                                                                                // Иначе
                    if ((ColumnType == "NUMERIC") || (ColumnType == "FLOAT") || (ColumnType == "CURRENCY") || (ColumnType == "INTEGER") || (ColumnType == "DOUBLE"))
                    Array.Sort(value, keys, new NumericComparerAsc());      //Используем стандартную сортировку
                else
                    Array.Sort(value, keys, new StringComparerAsc());      //Используем стандартную сортировку
                activeSortingType = SortingType.ASC;
                columnNameSort = ColumnName;
            }
            else
            {
                if (activeSortingType == SortingType.DESC)
                {
                    if ((ColumnType == "DATE") || (ColumnType == "DATETIME"))           //Если сортируемая колонка имеет тип Date илиDateTime
                        Array.Sort(value, keys, new DateComparerAsc());          //То используем сравнение со сцециальным DateComparer
                    else                                                                                // Иначе
                        if ((ColumnType == "NUMERIC") || (ColumnType == "FLOAT") || (ColumnType == "CURRENCY") || (ColumnType == "INTEGER") || (ColumnType == "DOUBLE"))
                        Array.Sort(value, keys, new NumericComparerAsc());      //Используем стандартную сортировку
                    else
                        Array.Sort(value, keys, new StringComparerAsc());      //Используем стандартную сортировку
                    activeSortingType = SortingType.ASC;
                }
                else
                {
                    if ((ColumnType == "DATE") || (ColumnType == "DATETIME"))           //Если сортируемая колонка имеет тип Date илиDateTime
                        Array.Sort(value, keys, new DateComparerDesc());          //То используем сравнение со сцециальным DateComparer
                    else                                                                                // Иначе
                        if ((ColumnType == "NUMERIC") || (ColumnType == "FLOAT") || (ColumnType == "CURRENCY") || (ColumnType == "INTEGER") || (ColumnType == "DOUBLE"))
                        Array.Sort(value, keys, new NumericComparerDesc());      //Используем стандартную сортировку
                    else
                        Array.Sort(value, keys, new StringComparerDesc());      //Используем стандартную сортировку
                    activeSortingType = SortingType.DESC;
                }
            }
        }

        public void SortArray(ref string[] value, ref int[] keys, string ColumnName, string ColumnType, SortingType sortingType)
        {
            //ColumnType = "DATE";
            if (sortingType == SortingType.ASC)
            {
                if ((ColumnType == "DATE") || (ColumnType == "DATETIME"))           //Если сортируемая колонка имеет тип Date илиDateTime
                    Array.Sort(value, keys, new DateComparerAsc());          //То используем сравнение со сцециальным DateComparer
                else
                    if ((ColumnType == "NUMERIC") || (ColumnType == "FLOAT") || (ColumnType == "CURRENCY") || (ColumnType == "INTEGER") || (ColumnType == "DOUBLE"))
                    Array.Sort(value, keys, new NumericComparerAsc());      //Используем стандартную сортировку
                else
                    Array.Sort(value, keys, new StringComparerAsc());      //Используем стандартную сортировку
            }
            else
                if (sortingType == SortingType.DESC)
            {
                if ((ColumnType == "DATE") || (ColumnType == "DATETIME"))           //Если сортируемая колонка имеет тип Date илиDateTime
                    Array.Sort(value, keys, new DateComparerDesc());          //То используем сравнение со сцециальным DateComparer
                else
                    if ((ColumnType == "NUMERIC") || (ColumnType == "FLOAT") || (ColumnType == "CURRENCY") || (ColumnType == "INTEGER") || (ColumnType == "DOUBLE"))
                    Array.Sort(value, keys, new NumericComparerDesc());      //Используем стандартную сортировку
                else
                    Array.Sort(value, keys, new StringComparerDesc());      //Используем стандартную сортировку
            }
            activeSortingType = sortingType;
            columnNameSort = ColumnName;
        }

    }
}
