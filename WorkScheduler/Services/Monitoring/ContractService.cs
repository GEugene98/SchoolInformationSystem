using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkScheduler.Models.Monitoring;
using WorkScheduler.Models.Monitoring.Shared;
using WorkScheduler.ViewModels;
using WorkScheduler.ViewModels.Monitoring;
using WorkScheduler.ViewModels.Monitoring.Shared;

namespace WorkScheduler.Services.Monitoring
{
    public class ContractService
    {
        protected Context Db;

        public ContractService(Context context)
        {
            Db = context;
        }

        public List<ContractViewModel> GetContracts(int schoolId)
        {
            return
                Db.Contracts
                    .Where(c => c.SchoolId == schoolId)
                    .OrderBy(c => c.Organization.Name)
                    .Select(contract => new ContractViewModel
                    {
                        Id = contract.Id,
                        OrganizationId = contract.OrganizationId,
                        Number = contract.Number,
                        SigningDate = contract.SigningDate,
                        Subject = contract.Subject,
                        SignedById = contract.SignedById,
                        Sum = contract.Sum,
                        Status = contract.Status,
                        ControlDate = contract.ControlDate,
                        Comment = contract.Comment,
                        SchoolId = contract.SchoolId
                    })
                    .ToList();
        }

        public long CreateContract(ContractViewModel contract)
        {
            var newContract = new Contract
            {
                OrganizationId = contract.OrganizationId,
                Number = contract.Number,
                SigningDate = contract.SigningDate,
                Subject = contract.Subject,
                SignedById = contract.SignedById,
                Sum = contract.Sum,
                Status = contract.Status,
                ControlDate = contract.ControlDate,
                Comment = contract.Comment,
                SchoolId = contract.SchoolId
            };

            Db.Contracts.Add(newContract);
            Db.SaveChanges();

            return newContract.Id;
        }

        public void UpdateContract(ContractViewModel contract)
        {
            var foundContract = Db.Contracts.FirstOrDefault(c => c.Id == contract.Id);

            if (contract == null)
            {
                throw new Exception();
            }

            foundContract.Number = contract.Number;
            foundContract.OrganizationId = contract.OrganizationId;
            foundContract.SignedById = contract.SignedById;
            foundContract.SigningDate = contract.SigningDate;
            foundContract.Status = contract.Status;
            foundContract.Subject = contract.Subject;
            foundContract.Sum = contract.Sum;

            Db.SaveChanges();
        }
        
        public void DeleteContract(long contractId)
        {
            var contract = Db.Contracts.FirstOrDefault(c => c.Id == contractId);

            if (contract == null)
            {
                throw new Exception();
            }

            Db.Remove(contract);

            Db.SaveChanges();
        }
    }
}
