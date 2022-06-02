using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    internal class AreaSelectablePictureBox : PictureBox
    {
        Rectangle? _selectedRectangle;
        Point? _dragStartedLocation;
        Point? currentLocation;

        readonly Pen currentRectanglePen = new Pen(new SolidBrush(Color.Blue), 2);

        readonly Pen previousRectanglePen = new Pen(new SolidBrush(Color.FromArgb(128, Color.Blue)), 2);
        readonly Pen newRectanglePen = new Pen(new SolidBrush(Color.Green), 2);
        readonly Pen badNewRectanglePen = new Pen(new SolidBrush(Color.Red), 2);

        public AreaSelectablePictureBox()
        {
            _selectedRectangle = null;
            _dragStartedLocation = null;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Debug.WriteLine("MouseDown - " + e.Location);
            currentLocation = _dragStartedLocation = e.Location;
            base.OnMouseDown(e);
        }

        Rectangle? CalculateRectangle(Point start, Point end)
        {
            int x = start.X;
            int y = start.Y;
            int width = end.X - x + 1;
            int height = end.Y - y + 1;

            if (width < 0)
            {
                width = Math.Abs(width);
                x -= width;
            }                
            if(height < 0)
            {
                height = Math.Abs(height);
                y -= height;
            }
            
            return new Rectangle(x, y, width, height);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Debug.WriteLine("MouseUp - " + e.Location);
            currentLocation = e.Location;

            if (_dragStartedLocation != null)
            {
                var rect = CalculateRectangle(_dragStartedLocation.Value, e.Location);

                if (rect == null)
                    return;

                _dragStartedLocation = null;
                _selectedRectangle = rect;
                Debug.WriteLine("MouseUp - new rectangle = " + _selectedRectangle);

                Invalidate();
            }
            base.OnMouseUp(e);  
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            currentLocation = e.Location;

            if (_dragStartedLocation.HasValue)
            {
                // 드래깅 중이면 계속 업데이트
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (_dragStartedLocation.HasValue)
            {
                // 기존 사각형 그리기
                if (_selectedRectangle.HasValue)
                    pe.Graphics.DrawRectangle(previousRectanglePen, _selectedRectangle.Value);

                // 드래기 중인 사각형 그리기
                if (currentLocation.HasValue)
                {
                    var rect = CalculateRectangle(_dragStartedLocation.Value, currentLocation.Value);
                    if (rect != null)
                        pe.Graphics.DrawRectangle(newRectanglePen, rect.Value);
                }

            }
            else if(_selectedRectangle.HasValue)
            {
                // 선택된 사각형 그리기
                pe.Graphics.DrawRectangle(currentRectanglePen, _selectedRectangle.Value);
            }
            base.OnPaint(pe);
        }

    }
}
