import { Button } from '@/components/ui/button';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { File } from 'lucide-react';
import { getDepartments } from './actions';
import { DepartmentsTable } from './sales-table';
import { DepartmentsNewForm } from './departments-new-form';

export default async function SalesPage({
  searchParams
}: {
  searchParams: { q: string; offset: string };
}) {
  const search = searchParams.q ?? '';
  const offset = searchParams.offset ?? 0;
  const { departments, newOffset, totalDepartments } = await getDepartments(
    search,
    Number(offset)
  );

  return (
    <Tabs defaultValue="all">
      <div className="flex items-center">
        <TabsList>
          <TabsTrigger value="all">All</TabsTrigger>
          <TabsTrigger value="active">Active</TabsTrigger>
          <TabsTrigger value="draft">Draft</TabsTrigger>
          <TabsTrigger value="archived" className="hidden sm:flex">
            Archived
          </TabsTrigger>
        </TabsList>
        <div className="ml-auto flex items-center gap-2">
          <Button size="sm" variant="outline" className="h-8 gap-1">
            <File className="h-3.5 w-3.5" />
            <span className="sr-only sm:not-sr-only sm:whitespace-nowrap">
              Export
            </span>
          </Button>
          <DepartmentsNewForm />
        </div>
      </div>
      <TabsContent value="all">
        {
          <DepartmentsTable
            departments={departments}
            offset={newOffset}
            totalDepartments={totalDepartments}
          />
        }
      </TabsContent>
    </Tabs>
  );
}
