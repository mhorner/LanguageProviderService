using AutoMapper;
using Core.Interfaces;
using Data.Interfaces;
using Entities;
using EntitiesUi;

namespace Core;

public class MessageCore : IMessageCore, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _iMapper;
 
        public MessageCore(IUnitOfWork inputUnitOfWork)
        {
            _unitOfWork = inputUnitOfWork;
            var config = ConfigurationAutomapper();
            _iMapper = config.CreateMapper();
        }
 
        private MapperConfiguration ConfigurationAutomapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Message, MessageUi>()
                    .ForMember(destination => destination.MessageId, opts => opts.MapFrom<Guid>(source => source.Id))
                    .ForMember(destination => destination.MessageKey, opts => opts.MapFrom<string>(source => source.Key))
                    .ForMember(destination => destination.MessageValue, opts => opts.MapFrom<string>(source => source.Value))
                    .ForMember(destination => destination.MessageCulture, opts => opts.MapFrom<string>(source => source.Culture))
                    .ReverseMap();
            });
 
            return config;
        }
 
        public async Task<bool> DeleteMessage(Guid id)
        {
            var objMessage = await _unitOfWork.Messages.GetMessageById(id);
            if (objMessage == null)
            {
                return false;
            }
 
            try
            {
                await _unitOfWork.Messages.DeleteMessage(id);
                await _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
 
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
 
        public async Task<MessageUi> GetMessageById(Guid id)
        {
            var objMessage = await _unitOfWork.Messages.GetMessageById(id);
            if (objMessage == null)
            {
                return null;
            }
            else
            {
                return _iMapper.Map<Message, MessageUi>(objMessage);
            }
        }
 
        public async Task<List<MessageUi>> GetMessages()
        {
            throw new NotImplementedException();
            // var lstMessages = await _unitOfWork.Messages.GetAllMessages();
            // return iMapper.Map<IEnumerable<Message>, List<MessageUi>>(lstMessages);
        }
 
        public async Task<bool> InsertMessage(MessageUi objMessageUi)
        {
            try
            {
                await _unitOfWork.Messages.InsertMessage(_iMapper.Map<MessageUi, Message>(objMessageUi));
                await _unitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
 
        public async Task<bool> UpdateMessage(MessageUi objMessageUi)
        {
            throw new NotImplementedException();
            // var objMessage = await _unitOfWork.Messages.GetMessageById(objMessageUi.MessageId);
            // if (objMessage == null)
            // {
            //     return false;
            // }
            // else
            // {
            //     objMessage.Key = objMessageUi.MessageKey;
            //     objMessage.Value = objMessageUi.MessageValue;
            //     await _unitOfWork.Messages.UpdateMessage(objMessage);
            //     try
            //     {
            //         await _unitOfWork.Save();
            //         return true;
            //     }
            //     catch
            //     {
            //         return false;
            //     }
            // }
        }
    }