using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FPTransformer
{
    public partial class MainForm : Form
    {
        private enum MatrixType
        { 
            PARALLEL,
            PERPENDICULAR,
            RATIO,
            RPLOT
        }
        private int[,] MyMatrixParallel1;
        private int[,] MyMatrixPerpendicular1;
        private int[,] MyMatrixRatio1;
        private int[,] MyMatrixParallel2;
        private int[,] MyMatrixPerpendicular2;
        private int[,] MyMatrixRatio2;
        private String MyPositionString;
        private const int COLUMNS = 24;
        private const int ROWS = 16;
        private const String MyVersion = "1.1";
        public MainForm()
        {
            InitializeComponent();
            InitMatrices();
            MyPositionString = "";
            VersionLabel.Text = MyVersion;
        }

        private void InitMatrix(out int[,] matrix)
        {
            matrix = new int[ROWS, COLUMNS];
            for(int i = 0; i < COLUMNS; i++)
                for(int j = 0; j < ROWS; j++)
                    matrix[j, i] = -1;
        }

        private void InitMatrixDouble(out double[,] matrix)
        {
            matrix = new double[ROWS, COLUMNS];
            for (int i = 0; i < COLUMNS; i++)
                for (int j = 0; j < ROWS; j++)
                    matrix[j, i] = -1.0;
        }

        private void InitMatrices()
        {
            InitMatrix(out MyMatrixParallel1);
            InitMatrix(out MyMatrixParallel2);
            InitMatrix(out MyMatrixPerpendicular1);
            InitMatrix(out MyMatrixPerpendicular2);
            InitMatrix(out MyMatrixRatio1);
            InitMatrix(out MyMatrixRatio2);
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private String GetDefaultOutputFileName(String inputFileName)
        {
            String outputString;
            if (inputFileName.Contains(".asc"))
            {
                outputString = inputFileName.Substring(0, inputFileName.IndexOf(".asc"));
            }
            else
            {
                outputString = inputFileName;
            }
            outputString = outputString + "_OldFormat";
            return outputString;
        }

        private String GetDefaultRPlotFileName(String inputFileName)
        {
            String outputString;
            if (inputFileName == null || inputFileName == "")
            {
                return "";
            }
            if (inputFileName.Contains(".asc"))
            {
                outputString = inputFileName.Substring(0, inputFileName.IndexOf(".asc"));
            }
            else
            {
                outputString = inputFileName;
            }
            outputString = outputString + ".rplot";
            return outputString;
        }

        private void BrowseInputFileButton_Click(object sender, EventArgs e)
        {
            if (InputFileOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                InputFileTextBox.Text = InputFileOpenFileDialog.FileName;
                OutputFileTextBox.Text = GetDefaultOutputFileName(InputFileTextBox.Text.Trim());
                RPlotFileTextBox.Text = GetDefaultRPlotFileName(InputFileTextBox.Text.Trim());
            }
        }

        private void AssignMatrix(int[,] matrix, int rowIndex, int columnIndex, int fileRow, string sValue)
        {
            int value;
            string errStr;
            if (!int.TryParse(sValue, out value))
            {
                errStr = "Error, could not read matrix value at line " + fileRow;
                throw new Exception(errStr);
            }
            matrix[rowIndex, columnIndex] = value;
        }

        private void AssignMatrices(string[] splittedLine, int rowIndex, int columnIndex, int fileRow)
        {
            AssignMatrix(MyMatrixParallel1, rowIndex, columnIndex, fileRow, splittedLine[1]);
            AssignMatrix(MyMatrixPerpendicular1, rowIndex, columnIndex, fileRow, splittedLine[2]);
            AssignMatrix(MyMatrixRatio1, rowIndex, columnIndex, fileRow, splittedLine[3]);
            AssignMatrix(MyMatrixParallel2, rowIndex, columnIndex, fileRow, splittedLine[4]);
            AssignMatrix(MyMatrixPerpendicular2, rowIndex, columnIndex, fileRow, splittedLine[5]);
            AssignMatrix(MyMatrixRatio2, rowIndex, columnIndex, fileRow, splittedLine[6]);
        }

        private void ReadFile()
        {
            StreamReader streamReader = null;
            String row, position;
            position firstBlockPosition, lastBlockPosition, tempPosition;
            string[] splittedLine;
            int iRow, iColumn, rowCounter = 0;
            int value;
            string errStr;
            try
            {
                InitMatrices();
                firstBlockPosition = new position(ROWS - 1, COLUMNS - 1);
                lastBlockPosition = new position(0, 0);
                streamReader = new StreamReader(InputFileTextBox.Text, Encoding.GetEncoding(1252));
                while ((row = streamReader.ReadLine()) != null)
                {
                    rowCounter++;
                    splittedLine = row.Split(new string[] { '\t'.ToString() }, StringSplitOptions.None);
                    position = splittedLine[0];
                    if (position.Length > 0 && position[0] >= 'A' && position[0] <= 'P' &&
                        int.TryParse(position.Substring(1).ToString().Trim(), out value))
                    {
                        iRow = (int)(position[0] - 'A');
                        if (iRow < 0 || iRow >= ROWS)
                        {
                            errStr = "Error, could not read matrix row number, line " + rowCounter;
                            throw new Exception(errStr);
                        }
                        if (!int.TryParse(position.Substring(1), out iColumn) || iColumn <= 0 || iColumn > COLUMNS)
                        {
                            errStr = "Error, could not read matrix column number, line " + rowCounter;
                            throw new Exception(errStr);
                        }
                        iColumn--;
                        tempPosition = new position(iRow, iColumn);
                        if (tempPosition < firstBlockPosition)
                        {
                            firstBlockPosition = tempPosition;
                        }
                        if (tempPosition > lastBlockPosition)
                        {
                            lastBlockPosition = tempPosition;
                        }
                        AssignMatrices(splittedLine, iRow, iColumn, rowCounter);
                    }
                }
                MyPositionString = MakePositionString(firstBlockPosition, lastBlockPosition, MyMatrixParallel1);
            }
            finally
            {
                streamReader.Close();
            }
        }

        private string MakePositionUpper(String input)
        {
            String str = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] >= 'a' && input[i] <= 'p')
                {
                    str = str + ((char)(input[i] + 'A' - 'a')).ToString();
                }
                else
                { 
                    str = str + input[i].ToString();
                }
            }
            return str;
        }

        private String MakePositionString(position firstBlockPos, position lastBlockPos, int[,] matrix)
        {
            String outputStr;
            position tempPos;
            PositionBlock positionBlock = new PositionBlock(firstBlockPos, lastBlockPos);
            outputStr = firstBlockPos.GetPositionString() + ":" + lastBlockPos.GetPositionString();
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                { 
                    if(matrix[i,j] > -1)
                    {
                        tempPos = new position(i, j);
                        if (!positionBlock.IsInside(tempPos))
                        {
                            outputStr = outputStr + "," + tempPos.GetPositionString();
                        }
                    }
                }
            }
            return outputStr;
        }

        private void WriteMatrixDouble(double[,] matrix, StreamWriter sw)
        {
            String row;
            // Header
            row = "";
            for (int i = 0; i < COLUMNS; i++)
            {
                row = row + '\t'.ToString() + (i + 1);
            }
            sw.WriteLine(row);
            for (int j = 0; j < ROWS; j++)
            {
                row = ((char)('A' + j)).ToString();
                for (int i = 0; i < COLUMNS; i++)
                {
                    if (matrix[j, i] > -1)
                    {
                        row = row + '\t'.ToString() + matrix[j, i].ToString();
                    }
                    else
                    {
                        row = row + '\t'.ToString();
                    }
                }
                sw.WriteLine(row);
            }
        }

        private void WriteMatrix(int[,] matrix, StreamWriter sw)
        {
            String row;
            // Header
            row = "";
            for (int i = 0; i < COLUMNS; i++)
            {
                row = row + '\t'.ToString() + (i + 1);
            }
            sw.WriteLine(row);
            for (int j = 0; j < ROWS; j++)
            {
                row = ((char)('A' + j)).ToString();
                for (int i = 0; i < COLUMNS; i++)
                {
                    if(matrix[j, i] > -1)
                    {
                        row = row + '\t'.ToString() + matrix[j, i].ToString();
                    }
                    else
                    {
                        row = row + '\t'.ToString();                        
                    }
                }
                sw.WriteLine(row);
            }        
        }

        private String GetMaxCps(int[,] matrix)
        {
            int maxCps = -1;
            for(int i = 0; i < ROWS; i++)
                for(int j = 0; j < COLUMNS; j++)
                    if(maxCps < matrix[i,j])
                        maxCps = matrix[i,j];
            return maxCps.ToString();
        
        }

        private String GetSet2String()
        { 
            String str;
            str = "Load time:	09/07/2009 01:59:51 PM\r\n" + 
"Read time:	09/07/2009 01:59:51 PM\r\n" + 
"Kinetic read cycle:	1  of  1\r\n" + 
"Read start delay time:	0	s\r\n" + 
"Time delay between kinetic reads:	<n/a>\r\n" + 
"Barcode:	ERROR\r\n" + 
"Method ID:	TamraSNP384\r\n" +
"Plate ID:	#6480\r\n" + 
"Comment:	";
            str = str + OutputFileTextBox.Text + "\r\n";
            str = str + "LR4_090825_PCR_7582694_15x\r\n" + 
"Microplate format:	ABgene_black_384\r\n" + 
"Shake time:	0	s\r\n" + 
"Temperature:	22	C\r\n" + 
"Instrument tag:	\r\n" + 
"Serial number:	AD2070\r\n" + 
"Read sequence:	row\r\n" + 
"Mode sequence:	well\r\n" + 
"Detection mode:	FPS\r\n" + 
"Light sensor:	HC-120\r\n" + 
"Excitation side:	Top\r\n" + 
"Emission side:	Top\r\n" + 
"Lamp:	Continuous\r\n" + 
"Readings per well:	1\r\n" + 
"Time between readings:	<n/a>\r\n" + 
"Integration time:	100000	us\r\n" + 
"Attenuator mode:	o\r\n" + 
"Motion settling time:	25	ms\r\n" + 
"Z Height:	4.7	mm	Middle\r\n" + 
"Excitation filter:	5	Tamra 550\r\n" + 
"Emission filter:	5	Tamra 580\r\n" + 
"Beamsplitter:	Top	R110/Tamra 520/490\r\n" + 
"Excitation polarizer filter:	s\r\n" + 
"Emission polarizer filter:	s\r\n" + 
"Detector counting:	Comparator\r\n" + 
"Flash lamp voltage:	<n/a>\r\n" + 
"Delay after flash:	<n/a>\r\n" + 
"Sensitivity setting:	2\r\n" + 
"A/D converter gain:	x1\r\n" + 
"Integrating gain:	x1\r\n" + 
"Max cps:	";
            str = str + GetMaxCps(MyMatrixParallel2) + " cps\r\n";
            str = str + "Min counts:	0	counts\r\n" + 
"Detection mode:	FPP\r\n" + 
"Light sensor:	HC-120\r\n" + 
"Excitation side:	Top\r\n" + 
"Emission side:	Top\r\n" + 
"Lamp:	Continuous\r\n" + 
"Readings per well:	1\r\n" + 
"Time between readings:	<n/a>\r\n" + 
"Integration time:	100000	us\r\n" + 
"Attenuator mode:	o\r\n" + 
"Motion settling time:	25	ms\r\n" + 
"Z Height:	4.7	mm	Middle\r\n" + 
"Excitation filter:	5	Tamra 550\r\n" + 
"Emission filter:	5	Tamra 580\r\n" + 
"Beamsplitter:	Top	R110/Tamra 520/490\r\n" + 
"Excitation polarizer filter:	s\r\n" + 
"Emission polarizer filter:	p\r\n" + 
"Detector counting:	Comparator\r\n" + 
"Flash lamp voltage:	<n/a>\r\n" + 
"Delay after flash:	<n/a>\r\n" + 
"Sensitivity setting:	2\r\n" + 
"A/D converter gain:	x1\r\n" + 
"Integrating gain:	x1\r\n" + 
"Max cps:	";
            str = str + GetMaxCps(MyMatrixPerpendicular2) + " cps\r\n";
            str = str + "Min counts:	0	counts\r\n";
            return str;
        }

        private String GetSet1String()
        {
            String str;
            str = "Load time:	09/07/2009 01:55:52 PM\r\n" + 
                    "Read time:	09/07/2009 01:55:52 PM\r\n" + 
                     "Kinetic read cycle:	1  of  1\r\n" + 
                    "Read start delay time:	0	s\r\n" + 
                    "Time delay between kinetic reads:	<n/a>\r\n" + 
                    "Barcode:	ERROR\r\n" + 
                    "Method ID:	R110SNP384\r\n" + 
                    "Plate ID:	#6480\r\n";
            str = str + "Comment:" + '\t'.ToString();
            str = str + OutputFileTextBox.Text + "\r\n";
            str = str + "Microplate format:	ABgene_black_384\r\n" +
"Shake time:	0	s\r\n" +
"Temperature:	21.7	C\r\n" +
"Instrument tag:    \r\n" +
"Serial number:	AD2070\r\n" +
"Read sequence:	row\r\n" +
"Mode sequence:	well\r\n" +
"Detection mode:	FPS\r\n" +
"Light sensor:	HC-120\r\n" +
"Excitation side:	Top\r\n" +
"Emission side:	Top\r\n" +
"Lamp:	Continuous\r\n" +
"Readings per well:	1\r\n" +
"Time between readings:	<n/a>\r\n" +
"Integration time:	100000	us\r\n" +
"Attenuator mode:	o\r\n" +
"Motion settling time:	25	ms\r\n" +
"Z Height:	4.7	mm	Middle\r\n" +
"Excitation filter:	3	BDF/R110 490\r\n" +
"Emission filter:	3	BDF/R110 520\r\n" +
"Beamsplitter:	Top	R110/Tamra 520/490\r\n" +
"Excitation polarizer filter:	s\r\n" +
"Emission polarizer filter:	s\r\n" +
"Detector counting:	Comparator\r\n" +
"Flash lamp voltage:	<n/a>\r\n" +
"Delay after flash:	<n/a>\r\n" +
"Sensitivity setting:	2\r\n" +
"A/D converter gain:	x1\r\n" +
"Integrating gain:	x1\r\n";
            str = str + "Max cps:	" + GetMaxCps(MyMatrixParallel1) + " cps\r\n";
            str = str + "Min counts:	0	counts\r\n" +
"Detection mode:	FPP\r\n" +
"Light sensor:	HC-120\r\n" +
"Excitation side:	Top\r\n" +
"Emission side:	Top\r\n" +
"Lamp:	Continuous\r\n" +
"Readings per well:	1\r\n" +
"Time between readings:	<n/a>\r\n" +
"Integration time:	100000	us\r\n" +
"Attenuator mode:	o\r\n" +
"Motion settling time:	25	ms\r\n" +
"Z Height:	4.7	mm	Middle\r\n" +
"Excitation filter:	3	BDF/R110 490\r\n" +
"Emission filter:	3	BDF/R110 520\r\n" +
"Beamsplitter:	Top	R110/Tamra 520/490\r\n" +
"Excitation polarizer filter:	s\r\n" +
"Emission polarizer filter:	p\r\n" +
"Detector counting:	Comparator\r\n" +
"Flash lamp voltage:	<n/a>\r\n" +
"Delay after flash:	<n/a>\r\n" +
"Sensitivity setting:	2\r\n" +
"A/D converter gain:	x1\r\n" +
"Integrating gain:	x1\r\n";
            str = str + "Max cps:	" + GetMaxCps(MyMatrixPerpendicular1) + " cps\r\n";
            str = str + "Min counts:	0	counts\r\n";

            return str;
        }

        private String GetPosition(int rowIndex, int columnIndex, bool capitals)
        {
            String str;
            if (capitals)
            {
                str = ((char)('A' + rowIndex)).ToString() + (1 + columnIndex).ToString();            
            }
            else
            {
                str = ((char)('a' + rowIndex)).ToString() + (1 + columnIndex).ToString();
            }
            return str;
        }

        private void WriteMatrixHeader(StreamWriter sw, MatrixType type, int matrixSet)
        {
            String gFactor, dataRow, units, sWellList;
            sWellList = "";
            gFactor = "";
            dataRow = "";
            units = "";
            switch (matrixSet)
            {
                case 1:
                    gFactor = "0.850000";
                    break;
                case 2:
                    gFactor = "1.000000";
                    break;
            }
            switch (type)
            { 
                case MatrixType.PARALLEL:
                    dataRow = "Data:" + '\t'.ToString() + "RAW DATA PARALLEL";
                    units = "cps";
                    sWellList = "Well list:" + '\t'.ToString() + "r0(" + MyPositionString +
                                ")" + " = cps(fps(" + MyPositionString  + "))";
                    break;
                case MatrixType.PERPENDICULAR:
                    dataRow = "Data:" + '\t'.ToString() + "RAW DATA PERPENDICULAR";
                    units = "cps";
                    sWellList = "Well list:" + '\t'.ToString() + "r1(" + MyPositionString + ")" + " = cps(fpp(" +
                                    MyPositionString + "))";
                    break;
                case MatrixType.RATIO:
                    dataRow = "Data:" + '\t'.ToString() + "RATIO (GFactor = " + gFactor + ")";
                    sWellList = "Well list:" + '\t'.ToString() + "r4(" + MakePositionUpper(MyPositionString) + 
                                ") = fp(fps(" + MyPositionString + "),fpp(" + MyPositionString + ")," + 
                                gFactor + ")";
                    units = "mP";
                    break;
                case MatrixType.RPLOT:
                    dataRow = "Data:" + '\t'.ToString() + "RATIO (GFactor = " + gFactor + ")";
                    sWellList = "Well list:" + '\t'.ToString() + "r4(" + MakePositionUpper(MyPositionString) +
                                ") = fp(fps(" + MyPositionString + "),fpp(" + MyPositionString + ")," +
                                gFactor + ")";
                    units = "R";
                    break;
            }
            sw.WriteLine(sWellList);
            sw.WriteLine(dataRow);
            sw.WriteLine("Units:" + '\t'.ToString() + units);
            sw.WriteLine("Display format:" + '\t'.ToString() + "%.0f");
        
        }

        private void WriteFile()
        {
            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter(OutputFileTextBox.Text, false, Encoding.GetEncoding(1252));
                streamWriter.Write(GetSet1String());
                WriteMatrixHeader(streamWriter, MatrixType.PARALLEL, 1);
                WriteMatrix(MyMatrixParallel1, streamWriter);
                WriteMatrixHeader(streamWriter, MatrixType.PERPENDICULAR, 1);
                WriteMatrix(MyMatrixPerpendicular1, streamWriter);
                WriteMatrixHeader(streamWriter, MatrixType.RATIO, 1);
                WriteMatrix(MyMatrixRatio1, streamWriter);
                streamWriter.Write(GetSet2String());
                WriteMatrixHeader(streamWriter, MatrixType.PARALLEL, 2);
                WriteMatrix(MyMatrixParallel2, streamWriter);
                WriteMatrixHeader(streamWriter, MatrixType.PERPENDICULAR, 2);
                WriteMatrix(MyMatrixPerpendicular2, streamWriter);
                WriteMatrixHeader(streamWriter, MatrixType.RATIO, 2);
                WriteMatrix(MyMatrixRatio2, streamWriter);
            }
            finally
            {
                streamWriter.Close();            
            }
        }

        private void CalcITot(out int[,] result, int[,] parMatrix, int[,] perpMatrix)
        {
            InitMatrix(out result);
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    if (parMatrix[i, j] == -1 || perpMatrix[i, j] == -1)
                    {
                        result[i, j] = -1;
                    }
                    else
                    {
                        result[i, j] = parMatrix[i, j] + 2 * perpMatrix[i, j];
                    }
                }
            }
        }

        private void CalcRmatrix(out double[,] Rresult, int[,] r110_itot, int[,] tamra_itot)
        {
            InitMatrixDouble(out Rresult);
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    if (r110_itot[i, j] == -1 || tamra_itot[i, j] == -1)
                    {
                        Rresult[i, j] = -1;
                    }
                    else
                    {
                        Rresult[i, j] = Math.Round(100*((double)tamra_itot[i, j]/(double)r110_itot[i, j]), 1);
                    }
                }
            }
        }

        private void WriteRPlotFile()
        {
            StreamWriter streamWriter = null;
            double[,] R;
            int[,] R110_ITot, Tamra_ITot;
            try
            {
                CalcITot(out R110_ITot, MyMatrixParallel1, MyMatrixPerpendicular1);
                CalcITot(out Tamra_ITot, MyMatrixParallel2, MyMatrixPerpendicular2);
                CalcRmatrix(out R, R110_ITot, Tamra_ITot);
                streamWriter = new StreamWriter(RPlotFileTextBox.Text, false, Encoding.GetEncoding(1252));
                streamWriter.Write(GetSet1String());
                WriteMatrixHeader(streamWriter, MatrixType.PARALLEL, 1);
                WriteMatrix(MyMatrixParallel1, streamWriter);
                WriteMatrixHeader(streamWriter, MatrixType.PERPENDICULAR, 1);
                WriteMatrix(MyMatrixPerpendicular1, streamWriter);
                WriteMatrixHeader(streamWriter, MatrixType.RPLOT, 1);
                WriteMatrixDouble(R , streamWriter);
                streamWriter.Write(GetSet2String());
                WriteMatrixHeader(streamWriter, MatrixType.PARALLEL, 2);
                WriteMatrix(MyMatrixParallel2, streamWriter);
                WriteMatrixHeader(streamWriter, MatrixType.PERPENDICULAR, 2);
                WriteMatrix(MyMatrixPerpendicular2, streamWriter);
                WriteMatrixHeader(streamWriter, MatrixType.RATIO, 2);
                WriteMatrix(MyMatrixRatio2, streamWriter);
            }
            finally
            {
                streamWriter.Close();
            }        
        }

        private void TransformButton_Click(object sender, EventArgs e)
        {
            if (InputFileTextBox.Text == "")
            {
                MessageBox.Show("Please specify an input file name!");
                return;
            }
            if (OutputFileTextBox.Text == "")
            {
                MessageBox.Show("Please specify an output file name!");
                return;
            }
            ReadFile();
            WriteFile();
            if (RPlotFileCheckBox.Checked)
            {
                WriteRPlotFile();
            }
            MessageBox.Show("The file(s) are created!");
        }

        private class position
        {
            private int MyRow;
            private int MyColumn;
            public position(int row, int column)
            {
                MyRow = row;
                MyColumn = column;
            }
            public position(String pos)
            {
                if (pos[0] >= 'a' && pos[0] <= 'p')
                {
                    MyRow = (int)(pos[0] - 'a');
                }
                else if (pos[0] >= 'A' && pos[0] <= 'P')
                {
                    MyRow = (int)(pos[0] - 'A');
                }
                else
                {
                    MyRow = -1;
                }
                if(!int.TryParse(pos.Substring(1).Trim(), out MyColumn))
                {
                    MyColumn = -1;
                }
            }

            public int GetRowIndex()
            {
                return  MyRow;
            }

            public int GetColumnIndex()
            {
                return MyColumn;
            }

            public String GetPositionString()
            {
                return ((char)('a' + MyRow)).ToString() + ((int)(MyColumn + 1)).ToString();
            }

            public override bool Equals(object obj)
            {
                return (this.MyColumn == ((position)obj).GetColumnIndex() && this.MyRow == ((position)obj).GetRowIndex());
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public static bool operator==(position pos1, position pos2)
            {
                return (pos1.MyColumn == pos2.GetColumnIndex() && pos1.MyRow == pos2.GetRowIndex());
            }

            public static bool operator !=(position pos1, position pos2)
            {
                return (pos1.MyColumn != pos2.GetColumnIndex() || pos1.MyRow != pos2.GetRowIndex());                
            }

            public static bool operator <(position pos1, position pos2)
            {
                if (pos1.GetRowIndex() < pos2.GetRowIndex())
                {
                    return true;
                }
                else if (pos1.GetRowIndex() == pos2.GetRowIndex() && pos1.GetColumnIndex() < pos2.GetColumnIndex())
                {
                    return true;
                }
                return false;
            }

            public static bool operator >(position pos1, position pos2)
            {
                if (!(pos1 < pos2) && !(pos1 == pos2))
                {
                    return true;
                }
                return false;
            }
            public static bool operator >=(position pos1, position pos2)
            {
                if ((pos1 > pos2) || (pos1 == pos2))
                {
                    return true;
                }
                return false;
            }
            public static bool operator <=(position pos1, position pos2)
            {
                if ((pos1 < pos2) || (pos1 == pos2))
                {
                    return true;
                }
                return false;
            }
        }

        private class PositionBlock
        {
            private position MyFirstPos;
            private position MyLastPos;
            public PositionBlock(position first, position last)
            {
                MyFirstPos = first;
                MyLastPos = last;
            }

            public bool IsInside(position pos)
            {
                if( pos.GetRowIndex() >= MyFirstPos.GetRowIndex() &&
                    pos.GetColumnIndex() >= MyFirstPos.GetColumnIndex() &&
                    pos.GetRowIndex() <= MyLastPos.GetRowIndex() &&
                    pos.GetColumnIndex() <= MyLastPos.GetColumnIndex())
                {
                    return true;
                }
                return false;
            }
        }

        private void RPlotFileCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RPlotFileCheckBox.Checked)
            {
                RPlotFileTextBox.Text = GetDefaultRPlotFileName(InputFileTextBox.Text.Trim());
                RPlotFileTextBox.ReadOnly = false;
            }
            else
            {
                RPlotFileTextBox.Text = "";
                RPlotFileTextBox.ReadOnly = true;
            }
        }
    }
}