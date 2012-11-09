﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace VitalStats.Model
{
    public static class ModelHelpers
    {
        public static List<string> UnpickleStrings(string str)
        {
            return new List<string>(str.Split(AppConstants.VALUE_DELIMITERS, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string PickleStrings(List<string> strs)
        {
            return String.Join(AppConstants.VALUE_DELIMITERS[0], strs.ToArray());
        }

        // Used to store doubles in string column
        public static List<double> UnpickleDoubles(string strs) 
        {
            List<double> vals = new List<double>() {};
            foreach (string s in strs.Split(AppConstants.VALUE_DELIMITERS, StringSplitOptions.RemoveEmptyEntries))
            {
                vals.Add(Convert.ToDouble(s));
            }
            return vals;
        }

        // Used to store doubles in string column
        public static string PickleDoubles(List<double> lst)
        {
            List<string> strs = new List<string>();
            foreach (double l in lst) 
            {
                strs.Add(String.Format("{0}", l));
            }
            return String.Join(AppConstants.VALUE_DELIMITERS[0], strs.ToArray());
        }


    }
}
