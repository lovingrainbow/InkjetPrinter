using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InkjetPrinter
{
    public partial class CoordinateConverter : UserControl
    {
        Point _MachinePoint = new Point();
        Point _SoftwarePoint = new Point();
        PointConverter _PointConverter = new PointConverter();
        Point _RunPoint = new Point();

        public delegate void RunPoint(Point point);

 
        public PointConverter PointConverter
        { get; set; }
        public Point SoftwarePoint
        { get; set; }
        public CoordinateConverter()
        {
            InitializeComponent();

        }

        public void refresh(Point MachinePoint)
        {
            _MachinePoint = MachinePoint;
            _SoftwarePoint = Coordiante.CoordinateConveter(_MachinePoint, _PointConverter);
            lbPointX.Text = Convert.ToString(_SoftwarePoint.X / 1000);
            lbPointY.Text = Convert.ToString(_SoftwarePoint.Y / 1000);
            lbPointZ.Text = Convert.ToString(_SoftwarePoint.Z / 1000);
        }

        private void btnORGXYSet_Click(object sender, EventArgs e)
        {
            _PointConverter.dX = _MachinePoint.X;
            _PointConverter.dY = _MachinePoint.Y;
        }

        private void btnORGRSet_Click(object sender, EventArgs e)
        {
            if (tbORGR.Text.Length > 0)
                _PointConverter.dR = double.Parse(tbORGR.Text);
            else
                _PointConverter.dR = 0;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (tbRunX.Text.Length > 0)
                _RunPoint.X = double.Parse(tbRunX.Text) * 1000;
            else
                _RunPoint.X = 0;
            if (tbRunY.Text.Length > 0)
                _RunPoint.Y = double.Parse(tbRunY.Text) * 1000;
            else
                _RunPoint.Y = 0;
            if (tbRunZ.Text.Length > 0)
                _RunPoint.Z = double.Parse(tbRunZ.Text) * 1000;
            else
                _RunPoint.Z = 0;
            
        }

    }
}
