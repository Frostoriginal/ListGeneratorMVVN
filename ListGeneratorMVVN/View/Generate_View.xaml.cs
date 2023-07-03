using ListGenerator.Core.ViewModels;
using ListGenerator.Core.ViewModels.Controls;
using System;
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

namespace ListGenerator.View
{
    /// <summary>
    /// Interaction logic for Generate_View.xaml
    /// </summary>
    public partial class Generate_View : UserControl
    {
        public Generate_View()
        {
            InitializeComponent();
            translateTheMonth();

            //var dt = new EmployeesPageViewModel();
            //DataContext = dt;
            //Hardcoded departments

            /*
            departments.Add(new Department() { Title = "RECEPCJA" });
            departments.Add(new Department() { Title = "KUCHNIA" });
            departments.Add(new Department() { Title = "BAR" });
            departments.Add(new Department() { Title = "SPA" });
            departments.Add(new Department() { Title = "PRACOWNIK DS. TECHNICZNYCH" });
            departments.Add(new Department() { Title = "POKOJOWE" });
            */
            foreach (var Department in DatabaseLocator.Database.Departments.ToList())
            {
                DepartmentList.Add(new DepartmentViewModel
                {
                    Id = Department.Id,
                    DepartmentName = Department.DepartmentName,

                });
            }

            DatePicker1.SelectedDate = timeSelected;
            // dt.NewEmployeeDepartment = newDepartment.Title;
            // dt.timeSelectedReference = timeSelected;
            // dt.timeSelectedString = timeSelected.ToString("MM.yyyy");

            //Error_Title.Text = dt.ErrorTitle;
            //ErrorMessageLocal = dt.ErrorMessage;

            // viewModelRelay = dt;

            // ErrorMessageLocal = viewModelRelay.ErrorMessage;
        }


        //public EmployeesPageViewModel viewModelRelay = new EmployeesPageViewModel();

        public string ErrorMessageLocal = "";

        #region Department related
        public class Department
        {
            public string Title { get; set; }
            public override string ToString() => Title;

        }
        public List<Department> departments = new List<Department>();
        public List<DepartmentViewModel> DepartmentList = new List<DepartmentViewModel>();

        Department newDepartment = new Department() { Title = "" };
        #endregion

        
        #region Date related
        public DateTime timeSelected = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);
        string selectedMonthTranslation = "";
        public void translateTheMonth()
        {
            if (timeSelected.Month == 1) selectedMonthTranslation = "Styczeń";
            if (timeSelected.Month == 2) selectedMonthTranslation = "Luty";
            if (timeSelected.Month == 3) selectedMonthTranslation = "Marzec";
            if (timeSelected.Month == 4) selectedMonthTranslation = "Kwiecień";
            if (timeSelected.Month == 5) selectedMonthTranslation = "Maj";
            if (timeSelected.Month == 6) selectedMonthTranslation = "Czerwiec";
            if (timeSelected.Month == 7) selectedMonthTranslation = "Lipiec";
            if (timeSelected.Month == 8) selectedMonthTranslation = "Sierpień";
            if (timeSelected.Month == 9) selectedMonthTranslation = "Wrzesień";
            if (timeSelected.Month == 10) selectedMonthTranslation = "Październik";
            if (timeSelected.Month == 11) selectedMonthTranslation = "Listopad";
            if (timeSelected.Month == 12) selectedMonthTranslation = "Grudzień";
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(selectedDateTextBlock != null)
            {
                timeSelected = (DateTime)DatePicker1.SelectedDate;
                selectedDateTextBlock.Text = timeSelected.ToString("MM.yyyy");
            }
            
            translateTheMonth();

        }
        #endregion

        #region FlowDocument and Document related buttons
        public FlowDocument defaultDoc = new FlowDocument();
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (DatabaseLocator.Database.Employees.ToList().Count > 0)
            {
                defaultDoc = CreateFlowDocument();
                defaultDoc.Name = "FlowDoc";
                FlowDocumentReader1.Document = defaultDoc;



            }
            else
            {
                ErrorMessageLocal = "Lista pracowników jest pusta!";
            }
            if (ErrorMessageLocal != "") DisplayErrorMessage();
        }

        public void PrintSimpleTextButton_Click(object sender, RoutedEventArgs e)
        {
            if (defaultDoc != null)
            {
                // Create a PrintDialog  
                PrintDialog printDlg = new PrintDialog();
                // Create IDocumentPaginatorSource from FlowDocument  
                IDocumentPaginatorSource idpSource = defaultDoc;
                // Call PrintDocument method to send document to printer  
                printDlg.ShowDialog();
                printDlg.PrintDocument(idpSource.DocumentPaginator, "Lista obecności");

            }
            else
            {
                ErrorMessageLocal = "Wygeneruj dokument";
            }
            if (ErrorMessageLocal != "") DisplayErrorMessage();
        }

        #region FlowDocument template        
        public FlowDocument CreateFlowDocument()
        {
            // Create a FlowDocument  
            FlowDocument doc = new FlowDocument();

            defaultDoc = doc;
            doc.Background = Brushes.White;

            doc.ColumnWidth = 400;
            doc.PagePadding = new Thickness(50);
            //Title page
            Section titlePage = new Section();
            titlePage.BreakPageBefore = true;
            titlePage.TextAlignment = TextAlignment.Center;
            Paragraph titleTitlePage = new Paragraph();
            Bold titleBold = new Bold();
            titleBold.FontSize = 80;
            titleBold.Inlines.Add(new Run($"\n\n\n\n\n\n\n\n\n\n\n\nLISTA\n\n\n OBECNOŚCI \n\n\n {selectedMonthTranslation.ToUpper()} {timeSelected.Year}"));
            titleTitlePage.Inlines.Add(titleBold);
            titlePage.Blocks.Add(titleTitlePage);
            doc.Blocks.Add(titlePage);


            //Department iterator

            //Department Title page
            foreach (var department in DepartmentList)
            {
                Section departmentTitlePage = new Section();
                departmentTitlePage.BreakPageBefore = true;
                departmentTitlePage.TextAlignment = TextAlignment.Center;
                Paragraph departmentTitle = new Paragraph();
                Bold departmentBold = new Bold();
                departmentBold.Inlines.Add(new Run($"\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n{department.DepartmentName}"));
                departmentTitle.Inlines.Add(departmentBold);
                departmentTitlePage.Blocks.Add(departmentTitle);
                departmentBold.FontSize = 80;
                doc.Blocks.Add(departmentTitlePage);

                //Employee iterator
                foreach (var employeeData in DatabaseLocator.Database.Employees.ToList()) //create a page for every employee
                {
                    if (employeeData.EmployeeDepartment.ToUpper() == department.DepartmentName.ToUpper())
                    {
                        Section singlePage = new Section();
                        #region Page styling variables
                        singlePage.BreakPageBefore = true;
                        singlePage.TextAlignment = TextAlignment.Center;
                        int paragraphFontSize = 16;
                        int headerFontSize = 16;
                        int dateFontSize = 20;
                        Thickness headerPadding = new Thickness(5);
                        Thickness tableBordersThickness = new Thickness(1);

                        #endregion

                        #region Title paragraph
                        // Create first Paragraph  
                        Paragraph p1 = new Paragraph();
                        // Create and add a new Bold, Italic and Underline  
                        Bold bld = new Bold();


                        bld.Inlines.Add(new Run($"Lista obecności \n (indywidualna karta czasu pracy) \n Imię i nazwisko: {employeeData.EmployeeName} {employeeData.EmployeeSurname} \n Miesiąc, rok: {selectedMonthTranslation} {timeSelected.Year} \n"));
                        p1.FontSize = paragraphFontSize;
                        p1.Inlines.Add(bld);
                        // Add Paragraph to Section  
                        singlePage.Blocks.Add(p1);
                        // Add Section to FlowDocument  
                        doc.Blocks.Add(singlePage);
                        #endregion
                        #region Table design
                        // Create the Table...
                        Table table1 = new Table();
                        // ...and add it to the FlowDocument Blocks collection.
                        table1.Margin = new Thickness(5);
                        // sec.Blocks.Add(table1);
                        singlePage.Blocks.Add(table1);


                        // Set some global formatting properties for the table.
                        table1.CellSpacing = 0;
                        table1.Background = Brushes.White;
                        table1.BorderBrush = Brushes.Black;
                        table1.BorderThickness = tableBordersThickness;
                        //Add columns
                        int numberOfColumns = 6;
                        for (int x = 0; x < numberOfColumns; x++)
                        {
                            table1.Columns.Add(new TableColumn());
                        }
                        // Create and add an empty TableRowGroup to hold the table's Rows.
                        table1.RowGroups.Add(new TableRowGroup());

                        // Alias the current working row for easy reference.

                        // Add  row.
                        table1.RowGroups[0].Rows.Add(new TableRow());
                        TableRow currentRow = table1.RowGroups[0].Rows[0];
                        // currentRow = table1.RowGroups[0].Rows[1];

                        // Global formatting for the header row.

                        currentRow.FontSize = headerFontSize;
                        currentRow.FontWeight = FontWeights.Bold;

                        // Header content
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("\nData"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Godzina rozpoczęcia pracy"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Podpis Pracownika"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Godzina zakończenia pracy"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Podpis Pracownika"))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Ilość godzin"))));

                        for (int i = 0; i < numberOfColumns; i++)
                        {
                            currentRow.Cells[i].BorderThickness = tableBordersThickness;
                            currentRow.Cells[i].BorderBrush = Brushes.Black;
                            currentRow.Cells[i].Padding = headerPadding;
                        }

                        // Date rows
                        for (int i = 1; i <= DateTime.DaysInMonth(timeSelected.Year, timeSelected.Month); i++)
                        {
                            string day = "";
                            string month = "";
                            if (i < 10)
                            {
                                day = $"0{i}";
                            }
                            else day = $"{i}";

                            if (timeSelected.Month < 10)
                            {
                                month = $"0{timeSelected.Month}";
                            }
                            else
                            {
                                month = $"{timeSelected.Month}";
                            }



                            table1.RowGroups[0].Rows.Add(new TableRow());
                            currentRow = table1.RowGroups[0].Rows[i];
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run($"{day}.{month}.{timeSelected.Year}"))));
                            currentRow.FontSize = dateFontSize;
                            currentRow.FontWeight = FontWeights.Normal;
                            currentRow.Cells[0].BorderThickness = tableBordersThickness;
                            currentRow.Cells[0].BorderBrush = Brushes.Black;

                            DateTime temp = new DateTime(timeSelected.Year, timeSelected.Month, i);
                            //Grey out weekends
                            if (temp.DayOfWeek == DayOfWeek.Saturday || temp.DayOfWeek == DayOfWeek.Sunday) currentRow.Background = new SolidColorBrush(Color.FromRgb(217, 217, 217));//Brushes.Gray; //RGB:217,217,217


                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells[1].BorderBrush = Brushes.Black;
                            currentRow.Cells[1].BorderThickness = tableBordersThickness;
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells[2].BorderBrush = Brushes.Black;
                            currentRow.Cells[2].BorderThickness = tableBordersThickness;
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells[3].BorderBrush = Brushes.Black;
                            currentRow.Cells[3].BorderThickness = tableBordersThickness;
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells[4].BorderBrush = Brushes.Black;
                            currentRow.Cells[4].BorderThickness = tableBordersThickness;
                            currentRow.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                            currentRow.Cells[5].BorderBrush = Brushes.Black;
                            currentRow.Cells[5].BorderThickness = tableBordersThickness;

                        }
                        #endregion
                    }
                }
            }


            return doc;
        }

        #endregion

        #endregion

        private void ButtonUpdateAdd_Click(object sender, RoutedEventArgs e)
        {
            //viewModelRelay.AddNewEmployee();

           // ErrorMessageLocal = viewModelRelay.ErrorMessage;
            if (ErrorMessageLocal != "") DisplayErrorMessage();

        }

        private void ButtonUpdateDelete_Click(object sender, RoutedEventArgs e)
        {
           // viewModelRelay.DeleteSelectedEmployee();

           // ErrorMessageLocal = viewModelRelay.ErrorMessage;
            if (ErrorMessageLocal != "") DisplayErrorMessage();

        }

        private void DisplayErrorMessage()
        {
            if (ErrorMessageLocal != "")
            {
                MessageBox.Show(ErrorMessageLocal, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ErrorMessageLocal = "";

        }

    }
}
