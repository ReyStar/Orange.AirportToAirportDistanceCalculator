UPDATE account 
SET firstname = @firstname, lastname = @lastname, revision = @revision + 1
WHERE id = @id 
--AND username = @username 
AND revision = @revision AND is_deleted != true
