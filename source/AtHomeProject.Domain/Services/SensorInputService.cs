using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtHomeProject.Data.Entities;
using AtHomeProject.Data.Interfaces;
using AtHomeProject.Domain.Interfaces;
using AtHomeProject.Domain.Models;
using AtHomeProject.Domain.Models.Pagination;
using AutoMapper;

namespace AtHomeProject.Domain.Services
{
    public class SensorInputService : ISensorInputService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SensorInputService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task InsertRangeAsync(IEnumerable<SensorInputModel> models)
        {
            var entities = _mapper.Map<IEnumerable<SensorInput>>(models).ToArray();

            await _unitOfWork.SensorInput.InsertRangeAsync(entities);
            await _unitOfWork.SaveAsync();
        }

        public async Task<PagedResult<SensorInputListModel>> GetPagedAsync(SensorInputRequest request)
        {
            var pagedResult = new PagedResult<SensorInputListModel>
            {
                CurrentPage = request.Page,
                PageSize = request.PageSize
            };

            var (results, rowCount) = await _unitOfWork.SensorInput.GetAsync(
                request.Page,
                request.PageSize,
                f => f.SerialNumber.Equals(request.SerialNumber) &&
                     f.DeviceRecorded.Date >=
                     (request.From != null ? request.From.Value.Date : f.DeviceRecorded.Date) &&
                     f.DeviceRecorded.Date <=
                     (request.To != null ? request.To.Value.Date : f.DeviceRecorded.Date),
                o => o.OrderByDescending(o => o.DeviceRecorded)
            );

            pagedResult.RowCount = rowCount;
            pagedResult.PageCount = (int)Math.Ceiling((double)pagedResult.RowCount / request.PageSize);
            pagedResult.Results = _mapper.Map<List<SensorInputListModel>>(results);

            return pagedResult;
        }
    }
}
