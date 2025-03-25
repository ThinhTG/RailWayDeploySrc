using AutoMapper;
using DAO.Contracts;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Pagging;
using Repositories.Product;
using Services.DTO;

namespace Services.Product
{
    public class BlindBoxService : IBlindBoxService
    {
        private readonly IBlindBoxRepository _repository;
        private readonly IPackageRepository _packageRepository;
        private readonly IMapper _mapper;

        public BlindBoxService(IBlindBoxRepository repository,IMapper mapper, IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BlindBox>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<PaginatedList<BlindBox>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<BlindBox> blindBoxes = _repository.GetAll().AsQueryable();
            return await PaginatedList<BlindBox>.CreateAsync(blindBoxes, pageNumber, pageSize);
        }
        public async Task<BlindBox> GetByIdAsync(Guid? id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<BlindBox>> GetBlindboxeByTypeSell(string typeSell)
        {
            return await _repository.GetBlindBoxByTypeSell(typeSell);
        }

        public async Task<BlindBox> AddAsync(AddBlindBoxDTO addBlindBoxDTO)
        {
            var package = await _packageRepository.GetPackageByIdAsync(addBlindBoxDTO.PackageId);
            var blindbox = new BlindBox
            {
                BlindBoxId = Guid.NewGuid(),
                PackageId = addBlindBoxDTO.PackageId,
                Package = package,
                CategoryId = package.CategoryId,
                BlindBoxName = addBlindBoxDTO.BlindBoxName,
                Price = addBlindBoxDTO.Price,
                Description = addBlindBoxDTO.Description,
                Stock = addBlindBoxDTO.Stock,
                Size = addBlindBoxDTO.Size,
                TypeSell = addBlindBoxDTO.TypeSell,
                Percent = addBlindBoxDTO.Percent,
                BlindBoxStatus = addBlindBoxDTO.BlindBoxStatus,
                CreatedAt = addBlindBoxDTO.CreatedAt.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(addBlindBoxDTO.CreatedAt, DateTimeKind.Utc)
                    : addBlindBoxDTO.CreatedAt.ToUniversalTime(),
                UpdatedAt = addBlindBoxDTO.UpdatedAt.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(addBlindBoxDTO.UpdatedAt, DateTimeKind.Utc)
                    : addBlindBoxDTO.UpdatedAt.ToUniversalTime()
            };
             await _repository.AddAsync(blindbox);
             package.BlindBoxes.Add(blindbox);
        await  _packageRepository.UpdatePackageAsync(package);
            
            return blindbox;
        }

        public async Task<BlindBox> UpdateAsync(Guid id, UpdateBlindBoxDTO updateBlindBoxDTO)
        {
            var blindbox = await _repository.GetByIdAsync(id);
            blindbox.PackageId = updateBlindBoxDTO.PackageId;
            blindbox.CategoryId = updateBlindBoxDTO.CategoryId;
            blindbox.BlindBoxName = updateBlindBoxDTO.BlindBoxName;
            blindbox.Price = updateBlindBoxDTO.Price;
            blindbox.Description = updateBlindBoxDTO.Description;
            blindbox.Stock = updateBlindBoxDTO.Stock;
            blindbox.Size = updateBlindBoxDTO.Size;
            blindbox.TypeSell = updateBlindBoxDTO.TypeSell;
            blindbox.Percent = updateBlindBoxDTO.Percent;
            blindbox.BlindBoxStatus = updateBlindBoxDTO.BlindBoxStatus;

            blindbox.UpdatedAt = updateBlindBoxDTO.UpdatedAt.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(updateBlindBoxDTO.UpdatedAt, DateTimeKind.Utc)
                : updateBlindBoxDTO.UpdatedAt.ToUniversalTime();
            await _repository.UpdateAsync(blindbox);
            return blindbox;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<PaginatedList<BlindBox>> GetAllFilter(string? searchByCategory,string? typeSell, string?size , string? searchByName, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
        {
            IQueryable<BlindBox> blindBoxes = _repository.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(searchByCategory))
            {
                blindBoxes = blindBoxes
                    .Include(b => b.Category)
                    .Where(b => b.Category.CategoryName.Contains(searchByCategory));
            }

            if (!string.IsNullOrEmpty(searchByName))
            {
                blindBoxes = blindBoxes.Where(b => b.BlindBoxName.Contains(searchByName));
            }

            if (minPrice.HasValue)
            {
                blindBoxes = blindBoxes.Where(b => b.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                blindBoxes = blindBoxes.Where(b => b.Price <= maxPrice.Value);
            }

            if(!string.IsNullOrEmpty(size))
            {
                blindBoxes = blindBoxes.Where(b => b.Size.Contains(size));
            }
            if(!string.IsNullOrEmpty(typeSell))
            {
                blindBoxes = blindBoxes.Where(b => b.TypeSell.Contains(typeSell));
            }



            return await PaginatedList<BlindBox>.CreateAsync(blindBoxes, pageNumber, pageSize);
        }


        //Mobile
        public async Task<IEnumerable<BlindBoxMobileResponse>> GetAllAsync(
            string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice,string? size)
        {
            IQueryable<BlindBox> blindBoxes = _repository.GetAll()
                .Include(b => b.Category)
                .Include(b => b.BlindBoxImages) // Include bảng chứa hình ảnh
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchByCategory))
            {
                blindBoxes = blindBoxes.Where(b => b.Category.CategoryName.Contains(searchByCategory));
            }

            if (!string.IsNullOrEmpty(searchByName))
            {
                blindBoxes = blindBoxes.Where(b => b.BlindBoxName.Contains(searchByName));
            }

            if (minPrice.HasValue)
            {
                blindBoxes = blindBoxes.Where(b => b.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                blindBoxes = blindBoxes.Where(b => b.Price <= maxPrice.Value);
            }

            if(!string.IsNullOrEmpty(size))
            {
                blindBoxes = blindBoxes.Where(b => b.Size.Contains(size));
            }

            var blindBoxMobileResponses = await blindBoxes.Select(b => new BlindBoxMobileResponse
            {
                BlindBoxId = b.BlindBoxId,
                ImageUrl = b.BlindBoxImages.Select(img => img.ImageUrl).FirstOrDefault(),
                BlindBoxName = b.BlindBoxName,
                Price = b.Price,
                Description = b.Description,
                Stock = b.Stock
            }).ToListAsync();

            return blindBoxMobileResponses;
        }

        public async Task<PaginatedList<BlindBox>> GetBlindboxeByTypeSellPaged(string typeSell, int pageNumber, int pageSize)
        {
            IQueryable<BlindBox> blindBoxes = (await _repository.GetBlindBoxByTypeSell(typeSell)).AsQueryable();
            return await PaginatedList<BlindBox>.CreateAsync(blindBoxes, pageNumber, pageSize);
        }

        public async Task<BlindBox> AddAsyncV2(AddBlindBoxDTOV2 addBlindBoxDTOV2)
        {
            // Fetch the Package using the PackageId from the DTO
            var package = await _packageRepository.GetPackageByIdAsync(addBlindBoxDTOV2.PackageId);

            var blindboxV2 = new BlindBox
            {

                BlindBoxId = Guid.NewGuid(),
                PackageId = addBlindBoxDTOV2.PackageId,
                CategoryId = package.CategoryId,
                BlindBoxName = addBlindBoxDTOV2.BlindBoxName,
                Price = addBlindBoxDTOV2.Price,
                Description = addBlindBoxDTOV2.Description,
                Stock = addBlindBoxDTOV2.Stock,
                Size = addBlindBoxDTOV2.Size,
                TypeSell = addBlindBoxDTOV2.TypeSell,
                Percent = addBlindBoxDTOV2.Percent,
                BlindBoxStatus = addBlindBoxDTOV2.BlindBoxStatus,
                CreatedAt = addBlindBoxDTOV2.CreatedAt.Kind == DateTimeKind.Unspecified
                   ? DateTime.SpecifyKind(addBlindBoxDTOV2.CreatedAt, DateTimeKind.Utc)
                   : addBlindBoxDTOV2.CreatedAt.ToUniversalTime(),
                UpdatedAt = addBlindBoxDTOV2.UpdatedAt.Kind == DateTimeKind.Unspecified
                   ? DateTime.SpecifyKind(addBlindBoxDTOV2.UpdatedAt, DateTimeKind.Utc)
                   : addBlindBoxDTOV2.UpdatedAt.ToUniversalTime()
            };
            await _repository.AddAsync(blindboxV2);
            return blindboxV2;
        }

        public async Task<BlindBox> UpdateAsyncV2(Guid id, UpdateBlindBoxDTOV2 updateBlindBoxDTOV2)
        {   
            var blindbox = await _repository.GetByIdAsync(id);
            

            blindbox.BlindBoxName = updateBlindBoxDTOV2.BlindBoxName;
            blindbox.Price = updateBlindBoxDTOV2.Price;
            blindbox.Description = updateBlindBoxDTOV2.Description;
            blindbox.Stock = updateBlindBoxDTOV2.Stock;
            blindbox.Size = updateBlindBoxDTOV2.Size;
            blindbox.TypeSell = updateBlindBoxDTOV2.TypeSell;
            blindbox.Percent = updateBlindBoxDTOV2.Percent;
            blindbox.BlindBoxStatus = updateBlindBoxDTOV2.BlindBoxStatus;

            blindbox.UpdatedAt = updateBlindBoxDTOV2.UpdatedAt.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(updateBlindBoxDTOV2.UpdatedAt, DateTimeKind.Utc)
                : updateBlindBoxDTOV2.UpdatedAt.ToUniversalTime();
            await _repository.UpdateAsync(blindbox);
            return blindbox;
        }

        public async Task<BlindBox> UpdateStatusAsync(Guid id, string status)
        {
            var blindbox = await _repository.GetByIdAsync(id);
            blindbox.BlindBoxStatus = "SoldOut";
            await _repository.UpdateAsync(blindbox);
            return blindbox;
        }

        public async Task<List<BlindBox>> GetBlindBoxLuckyWheel(Guid PackageId)
        {
            var blindboxes = await _repository.GetBlindBoxLuckyWheel(PackageId);
            return blindboxes.ToList();
        }
    }
}
