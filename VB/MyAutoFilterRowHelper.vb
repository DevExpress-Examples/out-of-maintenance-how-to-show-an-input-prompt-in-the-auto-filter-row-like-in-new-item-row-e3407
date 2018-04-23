Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.Utils

Namespace WindowsApplication1
	Public Class MyAutoFilterRowHelper
		Private ReadOnly _AutoFilterText As String
		Private ReadOnly _View As GridView
		Public Sub New(ByVal view As GridView, ByVal text As String)
			_View = view
			_AutoFilterText = text
			AddHandler view.CustomDrawCell, AddressOf view_CustomDrawCell
			AddHandler view.GridControl.Paint, AddressOf GridControl_Paint
		End Sub


		Public Function GetAutoFilterRowBounds() As Rectangle
			Dim viewInfo As GridViewInfo = TryCast(_View.GetViewInfo(), GridViewInfo)
			Dim rowInfo As GridDataRowInfo = TryCast(viewInfo.GetGridRowInfo(GridControl.AutoFilterRowHandle), GridDataRowInfo)
			If rowInfo Is Nothing OrElse (rowInfo.RowState And GridRowCellState.Focused) <> 0 Then
				Return Rectangle.Empty
			End If
			Dim r As Rectangle = rowInfo.DataBounds
			If r.X < viewInfo.ViewRects.ColumnPanelLeft Then
				r.X = viewInfo.ViewRects.ColumnPanelLeft
			End If
			If r.Right > viewInfo.ViewRects.Rows.Right Then
				r.Width = viewInfo.ViewRects.Rows.Right - r.X
			End If
			Return r
		End Function

		Private Sub GridControl_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)
			If (Not String.IsNullOrEmpty(_View.ActiveFilterString)) Then
				Return
			End If
			Dim bounds As Rectangle = GetAutoFilterRowBounds()
			If e.ClipRectangle.IntersectsWith(bounds) Then
				DrawAutoFilterRowText(_View, New GraphicsCache(e.Graphics), bounds)
			End If
		End Sub

		Private Sub view_CustomDrawCell(ByVal sender As Object, ByVal e As RowCellCustomDrawEventArgs)
			If e.RowHandle = GridControl.NewItemRowHandle Then
				e.Handled = True
			End If
		End Sub


		Private Sub DrawAutoFilterRowText(ByVal view As GridView, ByVal cache As GraphicsCache, ByVal r As Rectangle)
			Dim appearance As AppearanceObject = view.PaintAppearance.TopNewRow
			appearance.DrawBackground(cache, r)
			appearance.DrawString(cache, _AutoFilterText, r)
		End Sub

	End Class
End Namespace
