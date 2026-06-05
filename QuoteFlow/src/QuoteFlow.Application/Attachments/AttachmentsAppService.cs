using QuoteFlow.ApprovalHistories;
using QuoteFlow.Attachments.ParameterObjects;
using QuoteFlow.KeyAccounts;
using QuoteFlow.PriceOffers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;


namespace QuoteFlow.Attachments;

[RemoteService(IsEnabled = false)]

public class AttachmentsAppService : QuoteFlowAppService, IAttachmentsAppService
{

    protected IAttachmentRepository _attachmentRepository;
    protected AttachmentManager _attachmentManager;
    protected IKeyAccountRepository _keyAccountRepository;
    protected IPriceOfferRepository _priceOfferRepository;
    protected IGuidGenerator _guidGenerator;

    // Blob storage 
    protected readonly IBlobContainer<AttachmentContainer> _blobContainer;


    private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    public AttachmentsAppService(IHttpClientFactory httpClientFactory, Microsoft.Extensions.Configuration.IConfiguration configuration, IAttachmentRepository attachmentRepository, AttachmentManager attachmentManager, IBlobContainer<AttachmentContainer> blobContainer, IKeyAccountRepository keyAccountRepository, IGuidGenerator guidGenerator, IPriceOfferRepository priceOfferRepository)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _attachmentRepository = attachmentRepository;
        _attachmentManager = attachmentManager;
        _blobContainer = blobContainer;
        _keyAccountRepository = keyAccountRepository;
        _guidGenerator = guidGenerator;
        _priceOfferRepository = priceOfferRepository;
    }


    // Upload files
    public virtual async Task UploadFileAsync(
    [FromForm] List<IRemoteStreamContent> files,
    Guid requestId,
    string attachmentCode)
    {
        var yearMonth = DateTime.Now.ToString("yyyyMM");
        var output = new List<AttachmentDto>();

        foreach (var file in files)
        {
            // Build the original blob key for file storage
            var originalBlobKey = Path.Combine(attachmentCode.ToString(), yearMonth, file.FileName).Replace("\\", "/");
            var blobKey = originalBlobKey;

            if (file.ContentLength > (AttachmentConsts.AttachmentFileMaxSizeInMB * 1024 * 1024))
            {
                //throw new BusinessException(eRequestDomainErrorCodes.FileTooLarge)
                //    .WithData("file.FileName", file.FileName);
            }
            // Check if the file already exists in the blob
            int fileCount = 1;
            while (await _blobContainer.ExistsAsync(blobKey))
            {
                // If the file exists, append a number to the file name to make it unique
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                var newFileName = $"{fileNameWithoutExtension}_{fileCount}{fileExtension}";
                blobKey = Path.Combine(attachmentCode.ToString(), yearMonth, newFileName).Replace("\\", "/");
                fileCount++;
            }

            // Upload the file to blob storage
            await _blobContainer.SaveAsync(blobKey, file.GetStream()).ConfigureAwait(false);


            switch (attachmentCode)
            {
                case EntityTypes.KeyAccount:
                    var keyAccount = await _keyAccountRepository.GetAsync(requestId);

                    if (keyAccount != null)
                    {
                        keyAccount.AddedAttachmentAction(new KeyAccountAttachment(
                             _guidGenerator.Create(),
                             keyAccount.Id,
                             new AttachmentCreateParams
                             {
                                 RequestPart = string.Empty,
                                 FileName = file.FileName,
                                 FileNameDB = Path.GetFileName(blobKey),
                                 FilePath = blobKey,
                                 OfflineAttachment = false,
                                 Description = string.Empty
                             }
                         ));
                    }

                    await _keyAccountRepository.UpdateAsync(keyAccount, autoSave: true);
                    break;
                case EntityTypes.PriceOffer:
                    var priceOffer = await _priceOfferRepository.GetWithDetailsAsync(requestId);
                    if (priceOffer != null)
                    {
                        priceOffer.AddedAttachmentAction(new PriceOfferAttachment(
                            _guidGenerator.Create(),
                            priceOffer.Id,
                            new AttachmentCreateParams
                            {
                                RequestPart = string.Empty,
                                FileName = file.FileName,
                                FileNameDB = Path.GetFileName(blobKey),
                                FilePath = blobKey,
                                OfflineAttachment = false,
                                Description = string.Empty
                            }
                        ));
                    }
                    break;
                default:
                    break;
            }
            // Save file information to the database



        }


    }



    // Download
    public async Task<FileDto> GetFileByIdAsync(Guid id)
    {


        var attachment = await _attachmentRepository.GetAsync(id);

        var filePath = Path.Combine("C:\\Hosting\\QuoteFlow\\Attachments", "host", "Attachment-container", attachment.FilePath).Replace("/", "\\");

        try
        {
            var fileContent = await File.ReadAllBytesAsync(filePath);
            return new FileDto
            {
                FileName = attachment.FileName,
                ContentType = MimeMapping.MimeUtility.GetMimeMapping(attachment.FileName),
                Content = fileContent
            };
        }
        catch (Exception ex)
        {
            throw new EntityNotFoundException("Failed to fetch file content (Not found file in file path)");
        }

    }



    // Delete file
    public async Task DeleteFileAsync(Guid id)
    {


        var attachment = await _attachmentRepository.GetAsync(id);

        var filePath = Path.Combine("C:\\Hosting\\QuoteFlow\\Attachments", "host", "Attachment-container", attachment.FilePath).Replace("/", "\\");

        try
        {
            File.Delete(filePath);
            await _attachmentRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            File.Delete(filePath);
            await _attachmentRepository.DeleteAsync(id);
        }
    }

}