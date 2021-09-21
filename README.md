# SegmentProgressBar
Segmented Progress Bar for .NET (WinForms)

This is a progress bar user control for tracking the state of each segment in a series of jobs (eg downloads or file creation).

### Set up
```
this.progressBar.MaximumSegments = 100;

//progress colors
this.progressBar.NotStartedColor = Color.LightGray;
this.progressBar.InProgressColor = Color.Yellow;
this.progressBar.CompletedColor = Color.Green;
this.progressBar.FailedColor = Color.Red;

//border
this.progressBar.ShowBorder = true;
this.progressBar.BorderColor = Color.Black;

//text
this.progressBar.ShowText = true;
this.progressBar.TextColor = Color.Red

...

//fill test
Thread t = new Thread(FillTest);
t.Start();

...

//Example fill with 1000 tick delay between updates.
private async void FillTest() {
    for(int i = 0; i < 100; i++) {
        //set the index (starting at 0) and state
        this.progressBar.UpdateSegment(i, Nichnet.SegmentProgressBar.SegmentState.COMPLETED);
        
        //you can set the progress bar text, and also get the total segments in each state (view only)
        this.progressBar.Text = string.Format("{0} / {1}", this.progressBar.SegmentsCompleted, this.progressBar.MaximumSegments);
        
        await Task.Delay(1000);    
    }
}    
```

</br>
</br>
<p align="center">
  <img src="https://raw.githubusercontent.com/nichnet/SegmentProgressBar/main/Example.PNG" />
</p>
</br>

## Example Use Case: 

You've created a program that downloads Transport Stream files and want to monitor which parts (segments) are in the following states: not started, in progress, completed, failed. 

## Using
To use this, download the DLL file and add it to your Toolbox in Visual Studio Code.
.NET > 3.0 Required.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.


## License
[MIT](https://choosealicense.com/licenses/mit/)
