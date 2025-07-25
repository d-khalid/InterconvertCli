using System.CommandLine;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;

namespace InterconvertCli;

class Program
{
    public static string[] SupportedFormats =
    {
        "png",
        "jpeg",
        "webp"
    };
    // interconvert -f [newformat] -if [inputfile] -of [outputfile]
    static int Main(string[] args)
    {
        Option<string> newFormatOption = new("-f")
        {
            Description = "The new format for the image file.",
            Required = true,
            // Validation logic
            
            // DefaultValueFactory = result =>
            // {
            //     if (result.Tokens.Count == 0)
            //     {
            //         return string.Empty;
            //     }
            //     
            //     string fileFormat = result.Tokens.Single().Value;
            //     if (!SupportedFormats.Contains(fileFormat))
            //     {
            //         result.AddError($"\"{fileFormat}\" is not a supported image format.");
            //         return null;
            //     }
            //     else return fileFormat;
            // }
        };

        Option<string> inputFileOption = new("-if")
        {
            Description = "The input image file.",
            Required = true,
            // DefaultValueFactory = result =>
            // {
            //     if (result.Tokens.Count == 0)
            //     {
            //         return string.Empty;
            //     }
            //
            //     string filePath = result.Tokens.Single().Value;
            //     
            //     if (!File.Exists(filePath))
            //     {
            //         result.AddError($"\"{filePath}\" is not a file.");
            //         return null;
            //     }
            //     else return filePath;
            // }
        };
        Option<string> outputFileOption = new("-of")
        {
            Description = "The resulting image file from the conversion.",
            Required = true,
            // DefaultValueFactory = result =>
            // {
            //     string filePath = result.Tokens.Single().Value;
            //     return filePath;
            // }
        };
       
        RootCommand rootCommand = new("Convert an image file in one format to a specified format");
        
        rootCommand.Options.Add(newFormatOption);
        rootCommand.Options.Add(inputFileOption);
        rootCommand.Options.Add(outputFileOption);

        rootCommand.SetAction(parseResult => ConvertImage(
            parseResult.GetValue(newFormatOption),
            parseResult.GetValue(inputFileOption),
            parseResult.GetValue(outputFileOption)
            ));
        

        ParseResult parseResult = rootCommand.Parse(args);
        return parseResult.Invoke();
    }

    static void ConvertImage(string targetFormat, string inputFilePath, string outputFilePath)
    {
        // Validation
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine("Input file does not exist.");
            return;
        }

        if (!SupportedFormats.Contains(targetFormat))
        {
            Console.WriteLine("Unsupported format.");
            return;
        }

        Image image = Image.Load(inputFilePath);
        Console.WriteLine($"Dimensions: {image.Width} x {image.Height}");
        Console.WriteLine($"Format: {image.Metadata.DecodedImageFormat}");

        switch (targetFormat)
        {
            case "png":
                image.SaveAsPng(outputFilePath);
                break;
            case "jpeg":
                image.SaveAsJpeg(outputFilePath);
                break;
            case "webp":
                image.SaveAsWebp(outputFilePath);
                break;
        }
    }
}