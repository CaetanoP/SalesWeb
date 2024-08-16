import { Loader2 } from 'lucide-react';
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle
} from './ui/alert-dialog';

export function AreYouSure({
  description,
  open,
  setOpen,
  onConfirm,
  loading
}: {
  description: string;
  open: boolean;
  setOpen: (open: boolean) => void;
  onConfirm: () => void;
  loading?: boolean;
}) {
  return (
    <>
      <AlertDialog open={open} onOpenChange={setOpen}>
        <AlertDialogContent className="bg-slate-100">
          <AlertDialogHeader>
            <AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
            <AlertDialogDescription>{description}</AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction
              onClick={() => onConfirm()}
              disabled={loading}
              className="transition duration-500"
            >
              Continue
            </AlertDialogAction>
          </AlertDialogFooter>
          <div
            data-loading={loading}
            className="absolute inset-x-2 top-[calc(100%+1px)] -z-10 flex h-0 items-center justify-end overflow-hidden rounded-b-md bg-primary text-white transition-all duration-300 ease-linear data-[loading=true]:h-7"
          >
            <span className="me-3 text-sm">Loading...</span>
            <Loader2 className="me-3 h-4 w-4 animate-spin" />
          </div>
        </AlertDialogContent>
      </AlertDialog>
    </>
  );
}
