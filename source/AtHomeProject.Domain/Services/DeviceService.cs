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
    public class DeviceService : IDeviceService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task InsertRangeAsync(IEnumerable<DeviceModel> models)
        {
            var entities = _mapper.Map<IEnumerable<Device>>(models).ToArray();

            await _unitOfWork.Device.InsertRangeAsync(entities);
            await _unitOfWork.SaveAsync();
        }

        public async Task<DeviceModel> GetBySerialNumberAsync(string serialNumber)
        {
            var entity = await _unitOfWork.Device.FindAsync(d => d.SerialNumber == serialNumber);

            return _mapper.Map<DeviceModel>(entity);
        }

        public async Task<PagedResult<DeviceListModel>> GetPagedAsync(PaginationRequest request)
        {
            var result = new PagedResult<DeviceListModel>
            {
                CurrentPage = request.Page,
                PageSize = request.PageSize,
                RowCount = await _unitOfWork.Device.CountAsync
            };

            var pageCount = (double)result.RowCount / request.PageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var devices = await _unitOfWork.Device.GetAsync(
                request.Page,
                request.PageSize,
                q => q.OrderByDescending(o => o.LatestRegistration)
            );

            result.Results = _mapper.Map<List<DeviceListModel>>(devices);

            return result;
        }

        public async Task<IEnumerable<DeviceModel>> GetAsync()
        {
            var devices = await _unitOfWork.Device.GetAsync();

            return _mapper.Map<List<DeviceModel>>(devices);
        }

        public async Task<DeviceModel> InsertAsync(DeviceModel model)
        {
            var entity = _mapper.Map<Device>(model);

            await _unitOfWork.Device.InsertAsync(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<DeviceModel>(entity);
        }
    }
}
