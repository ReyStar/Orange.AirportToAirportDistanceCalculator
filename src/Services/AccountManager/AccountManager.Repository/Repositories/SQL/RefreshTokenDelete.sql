UPDATE refreshtoken SET is_deleted = true WHERE "account_id" = @accountId AND is_deleted = false
