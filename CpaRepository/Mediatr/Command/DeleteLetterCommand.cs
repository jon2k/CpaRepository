using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces.EF;
using Core.Interfaces.FileSystem;

namespace Web.Mediatr.Command
{
    public class DeleteLetterCommand : IRequest<int>
    {
        public int Id { get; set; }

        public class DeleteLetterCommandHandler : IRequestHandler<DeleteLetterCommand, int>
        {
            private readonly ILetterRepo _repo;
            private IFileService _fileService;

            public DeleteLetterCommandHandler(ILetterRepo repo, IFileService fileService)
            {
                _repo = repo;
                _fileService = fileService;
            }

            public async Task<int> Handle(DeleteLetterCommand request, CancellationToken cancellationToken)
            {
                var letter = _repo.GetById(request.Id);
                _fileService.DeleteFile(letter.PathLetter);
                return await _repo.DeleteAsync(letter);

            }
        }
    }
}
