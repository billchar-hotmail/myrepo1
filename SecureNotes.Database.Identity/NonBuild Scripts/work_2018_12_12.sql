﻿SELECT u.Id, u.UserName, r.Name
FROM [Identity].Users u
INNER JOIN [Identity].UserRoles ur ON u.Id = ur.UserId
INNER JOIN [Identity].Roles r ON ur.RoleId = r.Id
ORDER BY u.Id

WHERE email LIKE 'bill%'

