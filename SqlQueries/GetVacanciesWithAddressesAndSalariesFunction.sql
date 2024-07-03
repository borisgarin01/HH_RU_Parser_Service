CREATE OR REPLACE FUNCTION GetVacanciesWithAddressesAndSalaries(
    scheduleidInput VARCHAR(31), 
    experienceIdInput VARCHAR(31), 
    areaIdInput int
)
RETURNS TABLE (
    item_id VARCHAR,
    item_name VARCHAR,
    address_id VARCHAR,
    city VARCHAR,
    street VARCHAR,
    building VARCHAR,
    lat DOUBLE PRECISION,
    lng DOUBLE PRECISION,
    raw VARCHAR,
    metro_id varchar,
    salary_id varchar,
    salaryfrom int,
	salaryto int
) 
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        items.id AS item_id, 
        items.name AS item_name, 
        addresses.id AS address_id, 
        addresses.city AS city, 
        addresses.street AS street, 
        addresses.building AS building, 
        addresses.lat AS lat, 
        addresses.lng AS lng, 
        addresses.raw AS raw, 
        addresses.metroid AS metro_id, 
        salaries.id AS salary_id, 
        salaries.salaryfrom,
		salaries.salaryto
    FROM items 
    LEFT JOIN addresses ON items.addressid = addresses.id
    LEFT JOIN salaries ON items.id = salaries.id
    WHERE items.scheduleid = scheduleidInput AND items.experienceid = experienceIdInput
    
    UNION
    
    SELECT 
        items.id AS item_id, 
        items.name AS item_name, 
        addresses.id AS address_id, 
        addresses.city AS city, 
        addresses.street AS street, 
        addresses.building AS building, 
        addresses.lat AS lat, 
        addresses.lng AS lng, 
        addresses.raw AS raw, 
        addresses.metroid AS metro_id, 
        salaries.id AS salary_id, 
        salaries.salaryfrom,
		salaries.salaryto
    FROM items 
    RIGHT JOIN addresses ON items.addressid = addresses.id
    RIGHT JOIN salaries ON items.id = salaries.id
    WHERE items.scheduleid = scheduleidInput AND items.experienceid = experienceIdInput;
END;
$$;

SELECT * FROM GetVacanciesWithAddressesAndSalaries('remote', 'noExperience', -1);
