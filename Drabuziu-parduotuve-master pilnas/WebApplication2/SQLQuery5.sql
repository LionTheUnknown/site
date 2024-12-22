SELECT 
    fk.name AS ForeignKeyName,
    tp.name AS TableName,
    ref.name AS ReferencedTable
FROM 
    sys.foreign_keys AS fk
INNER JOIN 
    sys.tables AS tp ON fk.parent_object_id = tp.object_id
INNER JOIN 
    sys.tables AS ref ON fk.referenced_object_id = ref.object_id
WHERE 
    ref.name = 'produktai'; -- Replace with your table name