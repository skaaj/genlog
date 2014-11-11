﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Genlog
{
    /// <summary>
    /// Logique d'interaction pour MemoryTestView.xaml
    /// </summary>
    public partial class MemoryTestView : UserControl
    {

        private MemoryTestActivity _parent;

        public MemoryTestView(MemoryTestActivity parent)
        {
            InitializeComponent();

            _parent = parent;
        }

        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            _parent.SetView("challenge");
            _parent.timer.Start();
        }
    }
}