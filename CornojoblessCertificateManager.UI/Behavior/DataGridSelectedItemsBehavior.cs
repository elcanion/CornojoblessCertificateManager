using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace CornojoblessCertificateManager.UI.Behavior
{
    public static class DataGridSelectedItemsBehavior
    {
		public static readonly DependencyProperty SelectedItemsProperty =
		DependencyProperty.RegisterAttached(
			"SelectedItems",
			typeof(IList),
			typeof(DataGridSelectedItemsBehavior),
			new FrameworkPropertyMetadata(null, OnSelectedItemsChanged));

		public static IList GetSelectedItems(DependencyObject obj)
			=> (IList)obj.GetValue(SelectedItemsProperty);

		public static void SetSelectedItems(DependencyObject obj, IList value)
			=> obj.SetValue(SelectedItemsProperty, value);

		private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			if (d is not DataGrid dataGrid)
				return;

			dataGrid.SelectionChanged -= DataGrid_SelectionChanged;
			dataGrid.SelectionChanged += DataGrid_SelectionChanged;
		}

		private static void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (sender is not DataGrid dataGrid)
				return;

			var boundList = GetSelectedItems(dataGrid);
			if (boundList == null)
				return;

			boundList.Clear();

			foreach (var item in dataGrid.SelectedItems)
				boundList.Add(item);
		}
	}
}
