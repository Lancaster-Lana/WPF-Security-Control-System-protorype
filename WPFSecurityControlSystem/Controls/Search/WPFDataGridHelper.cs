using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UIPrototype.Controls.Search
{
    public static class WPFDataGridHelper
    {
        public static T GetVisualChild<T>(System.Windows.Media.Visual parent) where T : System.Windows.Media.Visual
        {
            T child = default(T);
            int numVisuals = System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                var v = (System.Windows.Media.Visual)System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        /// <summary>
        /// There is a simple method for getting the current (selected) row of the DataGrid:
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static System.Windows.Controls.DataGridRow GetSelectedRow(this DataGrid grid)
        {
            return (System.Windows.Controls.DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem);
        }

        /// <summary>
        /// get a row by its indices:
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static System.Windows.Controls.DataGridRow GetRow(this DataGrid grid, int index)
        {
            var row = (System.Windows.Controls.DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // May be virtualized, bring into view and try again.
                grid.UpdateLayout();
                grid.ScrollIntoView(grid.Items[index]);
                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        /// <summary>
        /// get a cell of a DataGrid by an existing row:
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static DataGridCell GetCell(this DataGrid grid, System.Windows.Controls.DataGridRow row, int column)
        {
            if (row != null)
            {
                var presenter = GetVisualChild<System.Windows.Controls.Primitives.DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    grid.ScrollIntoView(row, grid.Columns[column]);
                    presenter = GetVisualChild<System.Windows.Controls.Primitives.DataGridCellsPresenter>(row);
                }

                var cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }

        /// <summary>
        /// Or we can simply select a row by its indices:
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static DataGridCell GetCell(this DataGrid grid, int row, int column)
        {
            var rowContainer = grid.GetRow(row);
            return grid.GetCell(rowContainer, column);
        }

    }
}
