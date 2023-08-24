using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace MindMap
{
    // <summary>
    /// Utility class for file-related operations using JavaScript interop in Blazor.
    /// </summary>
    /// <remarks>
    /// This class provides static methods to perform file-related operations in a Blazor application
    /// using JavaScript interop. These methods allow saving, downloading, and loading files from the client-side.
#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
    public class FileUtil
#pragma warning restore CA1052 // Static holder types should be Static or NotInheritable
    {
        /// <summary>
        /// Asynchronously saves data as a file on the client-side.
        /// </summary>
        /// <param name="js">The IJSRuntime instance used for JavaScript interop.</param>
        /// <param name="data">The data to be saved as a file.</param>
        /// <param name="fileName">The name of the file to be saved.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async static Task SaveAs(IJSRuntime js, string data, string fileName)
        {
            await js.InvokeAsync<object>(
                "saveDiagram",
#pragma warning disable CA1305 // Specify IFormatProvider
                Convert.ToString(data), fileName).ConfigureAwait(true);
#pragma warning restore CA1305 // Specify IFormatProvider
        }
        /// <summary>
        /// Asynchronously initiates the download of a file on the client-side.
        /// </summary>
        /// <param name="js">The IJSRuntime instance used for JavaScript interop.</param>
        /// <param name="data">The data to be included in the file.</param>
        /// <param name="fileName">The name of the file to be downloaded.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async static Task DownloadFile(IJSRuntime js, string data, string fileName)
        {
            await js.InvokeAsync<object>(
                "downloadFile",
#pragma warning disable CA1305 // Specify IFormatProvider
                Convert.ToString(data), fileName).ConfigureAwait(true);
#pragma warning restore CA1305 // Specify IFormatProvider
        }
        /// <summary>
        /// Asynchronously triggers a click event on the client-side.
        /// </summary>
        /// <param name="js">The IJSRuntime instance used for JavaScript interop.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async static Task Click(IJSRuntime js)
        {
            await js.InvokeAsync<object>(
                "click").ConfigureAwait(true);
        }
        /// <summary>
        /// Asynchronously triggers a click event on the client-side.
        /// </summary>
        /// <param name="js">The IJSRuntime instance used for JavaScript interop.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async static Task<string> LoadFile(IJSRuntime js, object data)
        {
            return await js.InvokeAsync<string>(
                  "loadFile", data).ConfigureAwait(true);
        }
       
    }
}
