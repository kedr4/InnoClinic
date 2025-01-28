using Business.Abstractions.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    public readonly IFilesService _filesService;
    public DocumentsController(IFilesService filesService)
    {
        _filesService = filesService;
    }

    [Authorize]
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        using var stream = file.OpenReadStream();
        var blobFile = new BlobFile
        {
            Id = Guid.CreateVersion7(),
            Uri = file.FileName,
            Content = stream,
            ContentType = file.ContentType
        };

        var fileId = await _filesService.UploadFileAsync(blobFile, cancellationToken);

        return Ok(fileId);
    }

    [HttpGet("{fileId:guid}")]
    public async Task<IActionResult> GetFile(Guid fileId, CancellationToken cancellationToken)
    {
        var (fileStream, fileData) = await _filesService.GetFileAsync(fileId, cancellationToken);

        Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileData.FileName}\"");

        await fileStream.CopyToAsync(Response.Body, cancellationToken);
        await Response.Body.FlushAsync(cancellationToken);

        return new EmptyResult();
    }

    [Authorize]
    [HttpDelete("{fileId:guid}")]
    public async Task<IActionResult> DeleteFile(Guid fileId, CancellationToken cancellationToken)
    {
        await _filesService.DeleteFileAsync(fileId, cancellationToken);

        return NoContent();
    }

}
