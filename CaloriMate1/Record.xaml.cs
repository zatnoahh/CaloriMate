namespace CaloriMate1;

public partial class Record : ContentPage
{
    //string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CalorieMateRecord.txt");
	FirebaseHelper firebaseHelper = new FirebaseHelper();

    public Record()
	{
		InitializeComponent();
		//displayRecord.Text = File.ReadAllText(fileName);
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var allRecords = await firebaseHelper.GetAllFoodLogs();
        var today = datePicker.Date.ToString("dd/MM/yyyy");

        var filtered = allRecords.Where(r => r.Date == today).ToList();
        recordList.ItemsSource = filtered;

        int total = filtered.Sum(x => x.Calories);
        totalLabel.Text = $"Total Calories: {total} kcal";
    }


    private async void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        var selectedDate = e.NewDate.ToString("dd/MM/yyyy");
        var allRecords = await firebaseHelper.GetAllFoodLogs();

        var filtered = allRecords
            .Where(x => x.Date == selectedDate)
            .ToList();

        recordList.ItemsSource = filtered;

        int total = filtered.Sum(x => x.Calories);
        totalLabel.Text = $"Total Calories: {total} kcal";
    }

}