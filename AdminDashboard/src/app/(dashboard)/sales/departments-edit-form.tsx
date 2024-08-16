'use client';

import { Button } from '@/components/ui/button';
import {
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle
} from '@/components/ui/dialog';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { z } from 'zod';
import { DepartmentEntity, editDepartment } from './actions';

const formSchema = z.object({
  name: z.string().min(1, 'Name is required').max(20, 'Name is too long')
});

export function DepartmentsEditForm({
  department,
  setOpen
}: {
  department: DepartmentEntity;
  setOpen: (open: boolean) => void;
}) {
  if (!department)
    throw new Error(
      'DepartmentsEditForm needs a department reference to render'
    );

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      name: department.name
    }
  });

  function onSubmit(values: z.infer<typeof formSchema>) {
    try {
      editDepartment(+department.id, values.name);
      setOpen(false);
    } catch (error) {
      console.log(error);
      toast.error('Error editting department');
    }
    toast.success('Department editted successfully');
    console.log(values);
  }

  return (
    <DialogContent className="sm:max-w-[425px]">
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)}>
          <DialogHeader>
            <DialogTitle>Edit Deparment</DialogTitle>
            <DialogDescription>Edit Department information.</DialogDescription>
          </DialogHeader>
          <FormField
            control={form.control}
            name="name"
            render={({ field }) => (
              <div className="grid gap-4 py-4">
                <FormItem>
                  <div className="grid grid-cols-4 items-center gap-x-4 gap-y-1">
                    <FormLabel htmlFor="name" className="text-right">
                      Name
                    </FormLabel>
                    <FormControl>
                      <Input
                        placeholder="DepartmentName"
                        id="name"
                        className="col-span-3"
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className="col-span-3 col-start-2" />
                  </div>
                </FormItem>
              </div>
            )}
          ></FormField>
          <DialogFooter>
            <Button type="submit" disabled={form.formState.isSubmitting}>
              Edit
            </Button>
          </DialogFooter>
        </form>
      </Form>
    </DialogContent>
  );
}
