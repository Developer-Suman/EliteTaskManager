using AutoMapper;
using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.Task;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.DbContext;
using Project.DLL.Models;
using Project.DLL.Models.Task.SetUp;
using Project.DLL.RepoInterface;
using Project.DLL.Static.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project.BLL.Services.Implementation
{
    public class TaskServices : ITaskServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IHelpherMethods _helpherMethods;
        private readonly IimageRepository _iimageRepository;

        public TaskServices(IimageRepository iimageRepository, IHelpherMethods helpherMethods, ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork, IMapper mapper, IMemoryCacheRepository memoryCacheRepository)
        {
            _iimageRepository = iimageRepository;
            _helpherMethods = helpherMethods;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;

        }
        public async Task<Result<NickNameDTOs>> AddNickName(NickNameDTOs nickNameDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var nickName = new NickName(
                        newId,
                        nickNameDTOs.name,
                        nickNameDTOs.createdAt
                        );

                    await _unitOfWork.Repository<NickName>().AddAsync(nickName);
                    await _unitOfWork.SaveChangesAsync();
                    var resultDTOs = new NickNameDTOs(
                         nickName.Name,
                         nickName.CreatedAt

                     );
                    scope.Complete();

                    return Result<NickNameDTOs>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    throw new Exception("An exception occured while Adding");
                }

            }
        }
    }
}
