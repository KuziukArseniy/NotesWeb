using MediatR;
using AutoMapper;
using Notes.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Notes.Domain;
using Notes.Application.Common.Exceptions;
namespace Notes.Application.Notes.Queries.GetNoteDetails
{
    public class GetNoteDetailQueryHandler : IRequestHandler<GetNoteDetailQuery, NoteDetailsVm>
    {
        private readonly IMapper _mapper;
        private readonly INotesDbContext _dbContext;

        public GetNoteDetailQueryHandler(IMapper mapper, INotesDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<NoteDetailsVm> Handle(GetNoteDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Notes
                .FirstOrDefaultAsync(note =>
                note.Id == request.Id, cancellationToken);
            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Note), request.UserId);
            }
            return _mapper.Map<NoteDetailsVm>(entity);
        }
    }
}
