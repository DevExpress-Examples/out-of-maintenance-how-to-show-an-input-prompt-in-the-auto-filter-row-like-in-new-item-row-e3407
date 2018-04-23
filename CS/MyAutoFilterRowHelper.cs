using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Utils;

namespace WindowsApplication1
{
    public class MyAutoFilterRowHelper
    {
        private readonly string _AutoFilterText;
        private readonly GridView _View;
        public MyAutoFilterRowHelper(GridView view, string text)
        {
            _View = view;
            _AutoFilterText = text;
            view.CustomDrawCell += view_CustomDrawCell;
           // view.GridControl.Paint += GridControl_Paint;
            view.GridControl.PaintEx += GridControl_PaintEx;
        }

        private void GridControl_PaintEx(object sender, PaintExEventArgs e)
        {
            if (!string.IsNullOrEmpty(_View.ActiveFilterString))
                return;
            Rectangle bounds = GetAutoFilterRowBounds();
            if (e.ClipRectangle.IntersectsWith(bounds))
                DrawAutoFilterRowText(_View, e.Cache, bounds);
        }

        public Rectangle GetAutoFilterRowBounds()
        {
            GridViewInfo viewInfo = _View.GetViewInfo() as GridViewInfo;
            GridDataRowInfo rowInfo = viewInfo.GetGridRowInfo(GridControl.AutoFilterRowHandle) as GridDataRowInfo;
            if (rowInfo == null || (rowInfo.RowState & GridRowCellState.Focused) != 0)
                return Rectangle.Empty;
            Rectangle r = rowInfo.DataBounds;
            if (r.X < viewInfo.ViewRects.ColumnPanelLeft) r.X = viewInfo.ViewRects.ColumnPanelLeft;
            if (r.Right > viewInfo.ViewRects.Rows.Right) r.Width = viewInfo.ViewRects.Rows.Right - r.X;
            return r;
        }
        void view_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle == GridControl.NewItemRowHandle)
                e.Handled = true;
        }


        private void DrawAutoFilterRowText(GridView view, GraphicsCache cache, Rectangle r)
        {
            AppearanceObject appearance = view.PaintAppearance.TopNewRow;
            appearance.DrawBackground(cache, r);
            appearance.DrawString(cache, _AutoFilterText, r);
        }

    }
}
