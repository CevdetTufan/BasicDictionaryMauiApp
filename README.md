# BasicDictionaryMauiApp

BasicDictionaryMauiApp is a simple dictionary application built using .NET MAUI. It allows users to search for words and get their definitions.

## Features
- Word search functionality
- Simple and intuitive UI
- Lightweight and fast
- Uses MongoDB and local JSON storage for data persistence

## Pages
- **Main Page (MainPage.xaml)**: Used for word repetition practice, allowing users to review previously searched words.
- **Word Add Page (WordAddPage.xaml)**: Enables users to add new words and their definitions to the dictionary.
- **Word List Page (WordListPage.xaml)**: Allows users to search for words and delete them if needed.

## Installation
To run this project locally:

1. Clone the repository:
   ```sh
   git clone https://github.com/CevdetTufan/BasicDictionaryMauiApp.git
   cd BasicDictionaryMauiApp
   ```

2. Install dependencies and restore NuGet packages:
   ```sh
   dotnet restore
   ```

3. Build and run the application:
   ```sh
   dotnet build
   dotnet run
   ```

## Database
This project uses **MongoDB** to store word definitions and user data. Additionally, it supports local JSON storage for offline usage.
Ensure you have MongoDB installed and running before launching the application if you choose to use it.

## Contributing
Contributions are welcome! Feel free to open an issue or submit a pull request.

## License
This project is licensed under the MIT License.

