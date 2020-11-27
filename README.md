# BlazingApple.Survey :apple:

:fire:  A totally copacetic, easy-to-use front-end Blazor Survey package.

## About BlazingApples
BlazingApples is an open-source set of packages that aims to speed application development for Blazor WebAssembly organizations.

:zap: Check out the [demo site here](https://blazorsimplesurvey.azurewebsites.net/displaysurvey),https://blazorsimplesurvey.azurewebsites.net/displaysurvey [or this blog post on how the components work](https://blazorhelpwebsite.com/ViewBlogPost/44)!

:clap: Special thanks to [ADefWebServer](https://github.com/ADefWebserver/BlazorSimpleSurvey/commits?author=ADefWebserver) for creating the [BlazorSimpleSurvey](https://github.com/ADefWebserver/BlazorSimpleSurvey) demo application which this is based off of.

# Demo :video_camera:

<p align="center">
  <img alt="Demo of Copacetic" src="https://github.com/taylorchasewhite/copacetic-frontity/blob/master/readme/2020-09-04 - Frontity Site - Video.gif?raw=true">
</p>

# Installation :wrench:

## 1. Get the required dependencies.

1. On Client Project, right click and get to the NuGet Package Manager ("Manage NuGetPackages").
2. Install `BlazingApple.Survey`
![Survey Administration](https://github.com/BlazingApple/Survey/blob/main/README/InstallBlazingApplePackage.png?raw=true)
3. Add the following to `Program.cs's Main`:
```
			builder.Services.AddScoped<DialogService>();
			builder.Services.AddScoped<TooltipService>();
			builder.Services.AddScoped<NotificationService>();
			builder.Services.AddScoped<BlazorSurvey.Services.SurveyService>();
```

4. In your `index.html` file, add the required Radzen style and script:
```
    <link rel="stylesheet" href="_content/Radzen.Blazor/css/default-base.css"> <!-- this adds a lot of styles we don't want -->
    <script src="_content/Radzen.Blazor/Radzen.Blazor.js"></script>
```

## 2. Set up your server's API controller to receive the requests.
It is recommended to do this with EntityFrameworkCore to create the tables in my database and receive and process the request. This portion of the setup shows how to do this.

1. In your `Server` project, open `ApplicationDbContext`, add the following tables:
```
		using BlazingApples.Shared;
		...
    public DbSet<Survey> Surveys { get; set; }
		public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
		public DbSet<SurveyItem> SurveyItems { get; set; }
		public DbSet<SurveyItemOption> SurveyItemOptions { get; set; }
```
2. Create a `SurveysController.cs` in the `Controllers` directory. Feel free to copy [this controller](#).


# Usage.

1. Step 1
2. Step 2
3. Step 3

## Credits :white_flower:

- Build with love :blue_heart:, using [Radzen's Component Library](https://razor.radzen.com/) and [ADefWebServer](https://github.com/ADefWebserver/BlazorSimpleSurvey/commits?author=ADefWebserver)'s [BlazorSimpleSurvey](https://github.com/ADefWebserver/BlazorSimpleSurvey) as a starting point.

## Authors

1. [Taylor White](https://twitter.com/taychasewhite)

## License :scroll:

![License: GPL v2](https://img.shields.io/badge/License-GPL%20v2-blue.svg)

- **[GPLv2](https://www.gnu.org/licenses/old-licenses/gpl-2.0.en.html)**

## Home Page :camera:
![Survey Administration](https://github.com/BlazingApple/Survey/blob/main/README/SurveyAdmin.png?raw=true)

## Taking Surveys :camera:
![Taking a survey](https://github.com/BlazingApple/Survey/blob/main/README/Taking%20a%20Survey.png?raw=true)

## Monitoring Responses :camera:
![Survey Responses](https://github.com/BlazingApple/Survey/blob/main/README/SurveyResults.png?raw=true)

