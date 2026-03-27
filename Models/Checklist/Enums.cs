namespace MockServer.Models.Checklist;

public enum FulfilmentStatus
{
    EMPTY,
    TRUE,
    FALSE,
    NOT_RELEVANT
}

public enum QuestionType
{
    YES_OR_NO,
    YES_OR_NO_WITH_TEXT,
    COMPLETED_OR_NOT_RELEVANT,
    COMPLETED_OR_NOT_RELEVANT_WITH_TEXT
}

public enum RoleType
{
    NEW_EMPLOYEE,
    NEW_MANAGER,
    MANAGER_FOR_NEW_EMPLOYEE,
    MANAGER_FOR_NEW_MANAGER
}

public enum LifeCycle
{
    CREATED,
    ACTIVE,
    DEPRECATED,
    RETIRED
}

public enum Permission
{
    SUPERADMIN,
    ADMIN
}

public enum CommunicationChannel
{
    EMAIL,
    NO_COMMUNICATION
}
