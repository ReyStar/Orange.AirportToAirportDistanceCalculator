UPDATE account 
SET firstname = @firstname, lastname = @lastname, password_hash = @password_hash, password_salt = @password_salt, revision = @revision + 1
WHERE id = @id 
--AND username = @username 
AND revision = @revision AND is_deleted != true
