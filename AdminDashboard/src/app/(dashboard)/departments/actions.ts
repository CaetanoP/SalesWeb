'use server';

import { config } from '@/config.ts';
import { revalidateTag } from 'next/cache';

export interface DepartmentEnitity {
  id: string;
  name: string;
}

export async function deleteDepartment(id: number) {
  const response = await fetch(`${config.api_url}/Departments/${id}`, {
    method: 'DELETE'
  });
  revalidateTag('departments');
  return response.json();
}

export async function getDepartments(search: string, offset: number) {
  const response = await fetch(`${config.api_url}/Departments`, {
    cache: 'force-cache',
    next: { tags: ['departments'] }
  });
  const data = await response.json();

  const filteredData = data.filter((department: DepartmentEnitity) =>
    department.name.toLowerCase().includes(search.toLowerCase())
  );

  const paginatedData = filteredData.slice(offset, offset + 10);

  return {
    departments: paginatedData,
    newOffset: offset + 10,
    totalDepartments: filteredData.length
  };
}

export async function getDepartmentById(id: number) {
  const response = await fetch(`${config.api_url}/Departments/${id}`, {
    cache: 'force-cache',
    next: { tags: ['departments'] }
  });
  const data = await response.json();

  return data;
}

export async function addDepartment(name: string) {
  const response = await fetch(`${config.api_url}/Departments`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ name })
  });
  revalidateTag('departments');
  return response.json();
}
