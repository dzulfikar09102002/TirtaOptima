using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class User
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? More { get; set; }

    public bool? Gender { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Alamat { get; set; }

    public string? NomorTelepon { get; set; }

    public string? NikNip { get; set; }

    public long? RoleId { get; set; }

    public string? Photo { get; set; }

    public string? Jabatan { get; set; }

    public string? Email { get; set; }

    public bool? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public virtual ICollection<ActionType> ActionTypeCreatedByNavigations { get; set; } = new List<ActionType>();

    public virtual ICollection<ActionType> ActionTypeDeletedByNavigations { get; set; } = new List<ActionType>();

    public virtual ICollection<ActionType> ActionTypeUpdatedByNavigations { get; set; } = new List<ActionType>();

    public virtual ICollection<Bill> BillCreatedByNavigations { get; set; } = new List<Bill>();

    public virtual ICollection<Bill> BillDeletedByNavigations { get; set; } = new List<Bill>();

    public virtual ICollection<Bill> BillUpdatedByNavigations { get; set; } = new List<Bill>();

    public virtual ICollection<Collection> CollectionCreatedByNavigations { get; set; } = new List<Collection>();

    public virtual ICollection<Collection> CollectionDeletedByNavigations { get; set; } = new List<Collection>();

    public virtual ICollection<Collection> CollectionPenagihs { get; set; } = new List<Collection>();

    public virtual ICollection<Collection> CollectionUpdatedByNavigations { get; set; } = new List<Collection>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Criteria> CriteriaCreatedByNavigations { get; set; } = new List<Criteria>();

    public virtual ICollection<Criteria> CriteriaDeletedByNavigations { get; set; } = new List<Criteria>();

    public virtual ICollection<Criteria> CriteriaUpdatedByNavigations { get; set; } = new List<Criteria>();

    public virtual ICollection<Customer> CustomerCreatedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Customer> CustomerDeletedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<CustomerType> CustomerTypeCreatedByNavigations { get; set; } = new List<CustomerType>();

    public virtual ICollection<CustomerType> CustomerTypeDeletedByNavigations { get; set; } = new List<CustomerType>();

    public virtual ICollection<CustomerType> CustomerTypeUpdatedByNavigations { get; set; } = new List<CustomerType>();

    public virtual ICollection<Customer> CustomerUpdatedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Debt> DebtCreatedByNavigations { get; set; } = new List<Debt>();

    public virtual ICollection<Debt> DebtDeletedByNavigations { get; set; } = new List<Debt>();

    public virtual ICollection<Debt> DebtUpdatedByNavigations { get; set; } = new List<Debt>();

    public virtual ICollection<DebtsManagement> DebtsManagementCreatedByNavigations { get; set; } = new List<DebtsManagement>();

    public virtual ICollection<DebtsManagement> DebtsManagementDeletedByNavigations { get; set; } = new List<DebtsManagement>();

    public virtual ICollection<DebtsManagement> DebtsManagementUpdatedByNavigations { get; set; } = new List<DebtsManagement>();

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<District> DistrictCreatedByNavigations { get; set; } = new List<District>();

    public virtual ICollection<District> DistrictDeletedByNavigations { get; set; } = new List<District>();

    public virtual ICollection<District> DistrictUpdatedByNavigations { get; set; } = new List<District>();

    public virtual ICollection<FahpCalculation> FahpCalculationCreatedByNavigations { get; set; } = new List<FahpCalculation>();

    public virtual ICollection<FahpCalculation> FahpCalculationDeletedByNavigations { get; set; } = new List<FahpCalculation>();

    public virtual ICollection<FahpCalculation> FahpCalculationUpdatedByNavigations { get; set; } = new List<FahpCalculation>();

    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseDeletedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseUpdatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<Leader> LeaderCreatedByNavigations { get; set; } = new List<Leader>();

    public virtual ICollection<Leader> LeaderDeletedByNavigations { get; set; } = new List<Leader>();

    public virtual ICollection<Leader> LeaderUpdatedByNavigations { get; set; } = new List<Leader>();

    public virtual ICollection<Leader> LeaderUsers { get; set; } = new List<Leader>();

    public virtual ICollection<LetterCategory> LetterCategoryCreatedByNavigations { get; set; } = new List<LetterCategory>();

    public virtual ICollection<LetterCategory> LetterCategoryDeletedByNavigations { get; set; } = new List<LetterCategory>();

    public virtual ICollection<LetterCategory> LetterCategoryUpdatedByNavigations { get; set; } = new List<LetterCategory>();

    public virtual ICollection<Letter> LetterCreatedByNavigations { get; set; } = new List<Letter>();

    public virtual ICollection<Letter> LetterDeletedByNavigations { get; set; } = new List<Letter>();

    public virtual ICollection<Letter> LetterUpdatedByNavigations { get; set; } = new List<Letter>();

    public virtual ICollection<Payment> PaymentCreatedByNavigations { get; set; } = new List<Payment>();

    public virtual ICollection<Payment> PaymentDeletedByNavigations { get; set; } = new List<Payment>();

    public virtual ICollection<Payment> PaymentUpdatedByNavigations { get; set; } = new List<Payment>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Role> RoleCreatedByNavigations { get; set; } = new List<Role>();

    public virtual ICollection<Role> RoleDeletedByNavigations { get; set; } = new List<Role>();

    public virtual ICollection<Role> RoleUpdatedByNavigations { get; set; } = new List<Role>();

    public virtual ICollection<Status> StatusCreatedByNavigations { get; set; } = new List<Status>();

    public virtual ICollection<Status> StatusDeletedByNavigations { get; set; } = new List<Status>();

    public virtual ICollection<Status> StatusUpdatedByNavigations { get; set; } = new List<Status>();

    public virtual ICollection<StrategyResult> StrategyResultCreatedByNavigations { get; set; } = new List<StrategyResult>();

    public virtual ICollection<StrategyResult> StrategyResultDeletedByNavigations { get; set; } = new List<StrategyResult>();

    public virtual ICollection<StrategyResult> StrategyResultUpdatedByNavigations { get; set; } = new List<StrategyResult>();

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual ICollection<Village> VillageCreatedByNavigations { get; set; } = new List<Village>();

    public virtual ICollection<Village> VillageDeletedByNavigations { get; set; } = new List<Village>();

    public virtual ICollection<Village> VillageUpdatedByNavigations { get; set; } = new List<Village>();
}
