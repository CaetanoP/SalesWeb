'use client';

import { AreYouSure } from '@/components/are-you-sure';
import { Button } from '@/components/ui/button';
import { Dialog } from '@/components/ui/dialog';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuTrigger
} from '@/components/ui/dropdown-menu';
import { TableCell, TableRow } from '@/components/ui/table';
import { DialogTrigger } from '@radix-ui/react-dialog';
import { Edit, MoreHorizontal, Trash2 } from 'lucide-react';
import { useState } from 'react';
import { toast } from 'sonner';
import { deleteDepartment, DepartmentEnitity } from './actions';
import { DepartmentsEditForm } from './departments-edit-form';

export function Department({ department }: { department: DepartmentEnitity }) {
  const [open, setOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [openAlert, setOpenAlert] = useState(false);

  const onConfirmDelete = async () => {
    setLoading(true);
    try {
      await deleteDepartment(+department.id);
      toast.success('Department deleted successfully');
    } catch (error) {
      console.error(error);
      toast.error('Error deleting department');
    }
    setLoading(false);
  };

  return (
    <>
      <AreYouSure
        open={openAlert}
        setOpen={setOpenAlert}
        onConfirm={onConfirmDelete}
        description="Warning! This action is going to delete this Department and all the records of Sales and Sellers in it."
        loading={loading}
      />
      <Dialog open={open} onOpenChange={(open) => setOpen(open)}>
        <DepartmentsEditForm id={department.id} setOpen={setOpen} />
        <TableRow>
          <TableCell className="font-medium">{department.name}</TableCell>
          <TableCell align="right">
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button aria-haspopup="true" size="icon" variant="ghost">
                  <MoreHorizontal className="h-4 w-4" />
                  <span className="sr-only">Toggle menu</span>
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuLabel>Actions</DropdownMenuLabel>
                <DropdownMenuItem>
                  <DialogTrigger className="w-full text-left flex">
                    <Edit className="h-3.5 w-3.5 my-auto me-1.5" />
                    <span>Edit</span>
                  </DialogTrigger>
                </DropdownMenuItem>
                <DropdownMenuItem>
                  <div
                    onClick={() => setOpenAlert(true)}
                    className="w-full text-left flex hover:text-red-500 cursor-pointer"
                  >
                    <Trash2 className="h-3.5 w-3.5 my-auto me-1.5" />
                    Delete
                  </div>
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </TableCell>
        </TableRow>
      </Dialog>
    </>
  );
}
