namespace CaloriMate1
{
    public partial class MainPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        //string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CalorieMateRecord.txt");
        List<FoodLogEntry> dailyEntries = new();
        int totalCalories = 0;


        public MainPage()
        {
            InitializeComponent();
            dateLabel.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        void OnAddClicked(object sender, EventArgs e)
        {
            string food = inputFood.Text;
            string caloriesText = inputCalories.Text;
            string mealTime = mealTimePicker.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(food) || string.IsNullOrWhiteSpace(caloriesText) || string.IsNullOrWhiteSpace(mealTime))
            {
                outputEntry.Text = "Please fill all fields.";
                return;
            }

            if (!int.TryParse(caloriesText, out int calories))
            {
                outputEntry.Text = "Calories must be a number.";
                return;
            }

            // Add entry to list
            dailyEntries.Add(new FoodLogEntry
            {
                Date = selectDate.Date.ToString("dd/MM/yyyy"),
                Food = food,
                Calories = calories,
                MealTime = mealTime
            });

            // Update total calories
            int total = dailyEntries.Sum(x => x.Calories);
            outputEntry.Text = $"✔ Added {food} ({mealTime}) - {calories} kcal";
            totalCaloriesLabel.Text = $"Total Calories: {total} kcal";

            // Clear entry fields
            inputFood.Text = string.Empty;
            inputCalories.Text = string.Empty;
            mealTimePicker.SelectedIndex = -1;
        }

        void OnReset(object sender, EventArgs e)
        {
            inputFood.Text = string.Empty;
            inputCalories.Text = string.Empty;
            mealTimePicker.SelectedIndex = -1;
            selectDate.Date = DateTime.Now;
            outputEntry.Text = "No Entry Yet";

            dailyEntries.Clear();
            totalCalories = 0;
            totalCaloriesLabel.Text = "Total Calories: 0 kcal";
        }
              

        async void OnSaveRecord(object sender, EventArgs e)
        {
            if (dailyEntries.Count == 0)
            {
                await DisplayAlert("No Entry", "Please add at least one entry before saving.", "OK");
                return;
            }

            foreach (var entry in dailyEntries)
            {
                await firebaseHelper.AddFoodLog(entry.Date, entry.Food, entry.Calories, entry.MealTime);
            }

            await DisplayAlert("Saved", "All food records saved to Firebase!", "OK");

            dailyEntries.Clear();
            totalCaloriesLabel.Text = "Total Calories: 0 kcal";
            outputEntry.Text = "No Entry Yet";
        }

        void onDatePickerSelected(object sender, DateChangedEventArgs e)
        {
            var selectedDate = e.NewDate;

            // Example: Monday
            dayLabel.Text = selectedDate.ToString("dddd");

            // Example: 15 April 2025
            dateLabel.Text = selectedDate.ToString("dd MMMM yyyy");
        }

    }

}
